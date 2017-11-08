using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Renderers
{
    /// <summary>
    /// Picker cell.
    /// </summary>
    public class PickerCell:LabelCell
    {
        /// <summary>
        /// The page title property.
        /// </summary>
        public static BindableProperty PageTitleProperty =
            BindableProperty.Create(
                nameof(PageTitle),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(PickerCell),
                default(IEnumerable),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanging:ItemsSourceChanging
            );

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The display member property.
        /// </summary>
        public static BindableProperty DisplayMemberProperty =
            BindableProperty.Create(
                nameof(DisplayMember),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the display member.
        /// </summary>
        /// <value>The display member.</value>
        public string DisplayMember {
            get { return (string)GetValue(DisplayMemberProperty); }
            set { SetValue(DisplayMemberProperty, value); }
        }

        /// <summary>
        /// The selected items property.
        /// </summary>
        public static BindableProperty SelectedItemsProperty =
            BindableProperty.Create(
                nameof(SelectedItems),
                typeof(IList),
                typeof(PickerCell),
                default(IList),
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public IList SelectedItems {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// The max selected number property.
        /// </summary>
        public static BindableProperty MaxSelectedNumberProperty =
            BindableProperty.Create(
                nameof(MaxSelectedNumber),
                typeof(int),
                typeof(PickerCell),
                0,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the max selected number.
        /// </summary>
        /// <value>The max selected number.</value>
        public int MaxSelectedNumber {
            get { return (int)GetValue(MaxSelectedNumberProperty); }
            set { SetValue(MaxSelectedNumberProperty, value); }
        }

        /// <summary>
        /// The keep selected until back property.
        /// </summary>
        public static BindableProperty KeepSelectedUntilBackProperty =
            BindableProperty.Create(
                nameof(KeepSelectedUntilBack),
                typeof(bool),
                typeof(PickerCell),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.PickerCell"/> keep selected
        /// until back.
        /// </summary>
        /// <value><c>true</c> if keep selected until back; otherwise, <c>false</c>.</value>
        public bool KeepSelectedUntilBack {
            get { return (bool)GetValue(KeepSelectedUntilBackProperty); }
            set { SetValue(KeepSelectedUntilBackProperty, value); }
        }

        /// <summary>
        /// The accent color property.
        /// </summary>
        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(PickerCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the accent.
        /// </summary>
        /// <value>The color of the accent.</value>
        public Color AccentColor {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        /// <summary>
        /// The selected items order key property.
        /// </summary>
        public static BindableProperty SelectedItemsOrderKeyProperty =
            BindableProperty.Create(
                nameof(SelectedItemsOrderKey),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the selected items order key.
        /// </summary>
        /// <value>The selected items order key.</value>
        public string SelectedItemsOrderKey
        {
            get { return (string)GetValue(SelectedItemsOrderKeyProperty); }
            set { SetValue(SelectedItemsOrderKeyProperty, value); }
        }

        /// <summary>
        /// The selected command property.
        /// </summary>
        public static BindableProperty SelectedCommandProperty =
            BindableProperty.Create(
                nameof(SelectedCommand),
                typeof(ICommand),
                typeof(PickerCell),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the selected command.
        /// </summary>
        /// <value>The selected command.</value>
        public ICommand SelectedCommand {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
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

            if(SelectedItems == null){
                return string.Empty;
            }

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

            return string.Join(", ", sortedList.ToArray());
        }

        internal void InvokeCommand()
        {
            SelectedCommand?.Execute(SelectedItems);
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
