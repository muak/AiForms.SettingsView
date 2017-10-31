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

        public static BindableProperty SelectedItemsOrderKeyProperty =
            BindableProperty.Create(
                nameof(SelectedItemsOrderKey),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string SelectedItemsOrderKey
        {
            get { return (string)GetValue(SelectedItemsOrderKeyProperty); }
            set { SetValue(SelectedItemsOrderKeyProperty, value); }
        }

        //getters cache
        static ConcurrentDictionary<Type, Dictionary<string,Func<object, object>>> DisplayValueCache = new ConcurrentDictionary<Type, Dictionary<string,Func<object, object>>>();

        //DisplayMember getter
        internal Func<object, object> DisplayValue{
            get{
                if(_getters == null || DisplayMember == null){
                    return (obj) => obj;
                }
                if(_getters.ContainsKey(DisplayMember)){
                    return _getters[DisplayMember];
                }
                else{
                    return (obj) => obj;
                } 
            }
        }

        //OrderKey getter
        internal Func<object, object> KeyValue{
            get{
                if(_getters == null || SelectedItemsOrderKey == null){
                    return null;
                }
               
                if (_getters.ContainsKey(SelectedItemsOrderKey))
                {
                    return _getters[SelectedItemsOrderKey];
                }

                return null;
            }
        }

        internal string GetSelectedItemsText(){
            List<string> sortedList = null;

            if (KeyValue != null)
            {
                var dict = new Dictionary<object, string>();
                foreach (var item in SelectedItems)
                {
                    dict.Add(KeyValue(item), DisplayValue(item).ToString());
                }
                sortedList = dict.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            }
            else
            {
                var strList = new List<string>();
                foreach (var item in SelectedItems)
                {
                    strList.Add(DisplayValue(item).ToString());
                }
                sortedList = strList.OrderBy(x => x, new NaturalComparer()).ToList();
            }

            return string.Join(",", sortedList.ToArray());
        }

        Dictionary<string, Func<object, object>> _getters;

        static void ItemsSourceChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var controll = bindable as PickerCell;
            if(newValue == null){
                return;
            }

            controll.SetUpPropertyCache(newValue as IList);
        }

        // Create all propertiy getters
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

        void SetUpPropertyCache(IList itemsSource)
        {
            if(itemsSource.Count == 0){
                throw new ArgumentException("ItemsSource must have items more than or equal 1.");
            }

            _getters = DisplayValueCache.GetOrAdd(itemsSource[0].GetType(), CreateGetProperty);
        }


    }
}
