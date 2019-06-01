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
                typeof(IList),
                typeof(PickerCell),
                default(IList),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanging:ItemsSourceChanging
            );

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource {
            get { return (IList)GetValue(ItemsSourceProperty); }
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
        /// The sub display member property.
        /// </summary>
        public static BindableProperty SubDisplayMemberProperty =
            BindableProperty.Create(
                nameof(SubDisplayMember),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the sub display member.
        /// </summary>
        /// <value>The sub display member.</value>
        public string SubDisplayMember
        {
            get { return (string)GetValue(SubDisplayMemberProperty); }
            set { SetValue(SubDisplayMemberProperty, value); }
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

        /// <summary>
        /// The use natural sort property.
        /// </summary>
        public static BindableProperty UseNaturalSortProperty =
            BindableProperty.Create(
                nameof(UseNaturalSort),
                typeof(bool),
                typeof(PickerCell),
                false,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.PickerCell"/> use natural sort.
        /// </summary>
        /// <value><c>true</c> if use natural sort; otherwise, <c>false</c>.</value>
        public bool UseNaturalSort {
            get { return (bool)GetValue(UseNaturalSortProperty); }
            set { SetValue(UseNaturalSortProperty, value); }
        }

        /// <summary>
        /// The use auto value text property.
        /// </summary>
        public static BindableProperty UseAutoValueTextProperty =
            BindableProperty.Create(
                nameof(UseAutoValueText),
                typeof(bool),
                typeof(PickerCell),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.PickerCell"/> use auto value text.
        /// </summary>
        /// <value><c>true</c> if use auto value text; otherwise, <c>false</c>.</value>
        public bool UseAutoValueText
        {
            get { return (bool)GetValue(UseAutoValueTextProperty); }
            set { SetValue(UseAutoValueTextProperty, value); }
        }

        /// <summary>
        /// The use pick to close property.
        /// </summary>
        public static BindableProperty UsePickToCloseProperty =
            BindableProperty.Create(
                nameof(UsePickToClose),
                typeof(bool),
                typeof(PickerCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.PickerCell"/> use pick to close.
        /// </summary>
        /// <value><c>true</c> if use pick to close; otherwise, <c>false</c>.</value>
        public bool UsePickToClose
        {
            get { return (bool)GetValue(UsePickToCloseProperty); }
            set { SetValue(UsePickToCloseProperty, value); }
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

        internal Func<object,object> SubDisplayValue{
            get{
                if(_getters == null || SubDisplayMember == null){
                    return (obj) => null;
                }
                if(_getters.ContainsKey(SubDisplayMember)){
                    return _getters[SubDisplayMember];
                }
                else{
                    return (obj) => null;
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
                if(UseNaturalSort){
                    sortedList = dict.OrderBy(x => x.Key.ToString(),new NaturalComparer()).Select(x => x.Value).ToList();
                }
                else{
                    sortedList = dict.OrderBy(x => x.Key).Select(x => x.Value).ToList();
                }
            }
            else
            {
                var strList = new List<string>();
                foreach (var item in SelectedItems)
                {
                    strList.Add(DisplayValue(item).ToString());
                }
                var comparer = UseNaturalSort ? new NaturalComparer() : null;
                sortedList = strList.OrderBy(x => x, comparer).ToList();
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
            var typeArg = itemsSource.GetType().GenericTypeArguments;

            if(typeArg.Count() == 0){
                throw new ArgumentException("ItemsSource must be GenericType.");
            }

            // If the type is a system built-in-type, it doesn't create GetProperty.
            if(IsBuiltInType(typeArg[0]))
            {
                _getters = null;
                return;
            }

            _getters = DisplayValueCache.GetOrAdd(typeArg[0], CreateGetProperty);
        }

        bool IsBuiltInType(Type type)
        {
            var name = type.FullName;
            switch (name)
            {
                case "System.Boolean":
                case "System.Byte":
                case "System.SByte":
                case "System.Char":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                case "System.String":
                case "System.Object":
                    return true;
                default:
                    return false;
            }
        }
    }
}
