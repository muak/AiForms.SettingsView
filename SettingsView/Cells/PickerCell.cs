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
                defaultBindingMode: BindingMode.OneWay
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
                defaultBindingMode: BindingMode.OneWay
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

        static ConcurrentDictionary<Type, Func<object, object>> DisplayValueCache = new ConcurrentDictionary<Type, Func<object, object>>();

        internal Func<object, object> DisplayValue;

        Func<object,object> CreateGetProperty(Type t)
        {
            var prop = t.GetRuntimeProperties()
                                .Where(x => x.DeclaringType == t && x.Name == DisplayMember).FirstOrDefault();

            var target = Expression.Parameter(typeof(object), "target");

            var body = Expression.PropertyOrField(Expression.Convert(target, t), prop.Name);

            var lambda = Expression.Lambda<Func<object, object>>(
                Expression.Convert(body, typeof(object)), target
            );

            return lambda.Compile();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == ItemsSourceProperty.PropertyName || 
               propertyName == DisplayMemberProperty.PropertyName){
               SetUpPropertyCache(); 
            }
        }

        void SetUpPropertyCache()
        {
            if(ItemsSource == null){
                return;
            }

            var list = ItemsSource as IList;

            if(list.Count == 0){
                throw new ArgumentException("ItemsSource must have items more than or equal 1.");
            }

            if(string.IsNullOrEmpty(DisplayMember)){
                DisplayValue = (obj) => obj;
            }
            else{
                DisplayValue = DisplayValueCache.GetOrAdd(list[0].GetType(),CreateGetProperty);
            }
        }
    }
}
