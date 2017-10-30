using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class PickerCell:LabelCell
    {
        public static BindableProperty PageTitleProperty =
            BindableProperty.Create(
                nameof(PageTitle),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string PageTitle {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(PickerCell),
                default(IEnumerable),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanging:ItemsSourceChanging
            );

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static BindableProperty DisplayMemberProperty =
            BindableProperty.Create(
                nameof(DisplayMember),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanging:DisplayMemberChanging
            );

        public string DisplayMember {
            get { return (string)GetValue(DisplayMemberProperty); }
            set { SetValue(DisplayMemberProperty, value); }
        }

        public static BindableProperty SelectedItemsProperty =
            BindableProperty.Create(
                nameof(SelectedItems),
                typeof(IList),
                typeof(PickerCell),
                default(IList),
                defaultBindingMode: BindingMode.TwoWay
            );

        public IList SelectedItems {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static BindableProperty MaxSelectedNumberProperty =
            BindableProperty.Create(
                nameof(MaxSelectedNumber),
                typeof(int),
                typeof(PickerCell),
                0,
                defaultBindingMode: BindingMode.OneWay
            );

        public int MaxSelectedNumber {
            get { return (int)GetValue(MaxSelectedNumberProperty); }
            set { SetValue(MaxSelectedNumberProperty, value); }
        }

        public static BindableProperty KeepSelectedUntilBackProperty =
            BindableProperty.Create(
                nameof(KeepSelectedUntilBack),
                typeof(bool),
                typeof(PickerCell),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool KeepSelectedUntilBack {
            get { return (bool)GetValue(KeepSelectedUntilBackProperty); }
            set { SetValue(KeepSelectedUntilBackProperty, value); }
        }

        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(PickerCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color AccentColor {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        //getters cache
        static ConcurrentDictionary<Type, Dictionary<string,Func<object, object>>> DisplayValueCache = new ConcurrentDictionary<Type, Dictionary<string,Func<object, object>>>();

        internal Func<object, object> DisplayValue = (obj)=>obj;

        static void ItemsSourceChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var controll = bindable as PickerCell;
            if(newValue == null ||  string.IsNullOrEmpty(controll.DisplayMember)){
                return;
            }

            controll.SetUpPropertyCache(newValue as IList,controll.DisplayMember);
        }

        static void DisplayMemberChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var controll = bindable as PickerCell;
            if(controll.ItemsSource == null || string.IsNullOrEmpty((string)newValue)){
                return;
            }

            controll.SetUpPropertyCache(controll.ItemsSource as IList,(string)newValue);
        }

        // Create all property getter
        Dictionary<string,Func<object,object>> CreateGetProperty(Type t)
        {           
            var prop =t.GetRuntimeProperties()
                                .Where(x => x.DeclaringType == t && !x.Name.StartsWith("_", StringComparison.Ordinal));

            var target = Expression.Parameter(typeof(object), "target");

            var dictGetter = new Dictionary<string, Func<object, object>>();

            foreach (var p in prop) {

                var body = Expression.PropertyOrField(Expression.Convert(target,t), p.Name);

                var lambda = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(body,typeof(object)),target
                );

                dictGetter[p.Name] = lambda.Compile();
            }

            return dictGetter;
        }

        void SetUpPropertyCache(IList itemsSource,string displayMember)
        {
            if(itemsSource.Count == 0){
                throw new ArgumentException("ItemsSource must have items more than or equal 1.");
            }

            var getters = DisplayValueCache.GetOrAdd(itemsSource[0].GetType(), CreateGetProperty);
            if(getters.ContainsKey(displayMember)){
                DisplayValue = getters[displayMember];
            }
            else{
                DisplayValue = (obj) => obj;
            }
        }
    }
}
