using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace AiForms.Renderers
{
    public partial class SettingsView
    {
        public event EventHandler<DropEventArgs> ItemDropped;

        public static BindableProperty ItemDroppedCommandProperty = BindableProperty.Create(
            nameof(ItemDroppedCommand),
            typeof(ICommand),
            typeof(SettingsView),
            default(ICommand),
            defaultBindingMode: BindingMode.OneWay
        );

        public ICommand ItemDroppedCommand
        {
            get { return (ICommand)GetValue(ItemDroppedCommandProperty); }
            set { SetValue(ItemDroppedCommandProperty, value); }
        }

        /// <summary>
        /// The background color property.
        /// </summary>
        public static new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );
        
        /// <summary>
        /// A color of out of region and entire region. They contains header, footer and cell (in case android).
        /// </summary>
        /// <value>The color of the background.</value>
        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// The separator color property.
        /// </summary>
        public static BindableProperty SeparatorColorProperty =
            BindableProperty.Create(
                nameof(SeparatorColor),
                typeof(Color),
                typeof(SettingsView),
                Color.FromRgb(199, 199, 204),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Row separator color.
        /// </summary>
        /// <value>The color of the separator.</value>
        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        /// <summary>
        /// The selected color property.
        /// </summary>
        public static BindableProperty SelectedColorProperty =
            BindableProperty.Create(
                nameof(SelectedColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Cell backgraound color when row is selected.
        /// </summary>
        /// <value>The color of the selected.</value>
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        /// <summary>
        /// The header padding property.
        /// </summary>
        public static BindableProperty HeaderPaddingProperty =
            BindableProperty.Create(
                nameof(HeaderPadding),
                typeof(Thickness),
                typeof(SettingsView),
                new Thickness(14, 8, 8, 8),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section header padding
        /// </summary>
        /// <value>The header padding.</value>
        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        /// <summary>
        /// The header text color property.
        /// </summary>
        public static BindableProperty HeaderTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section header text color
        /// </summary>
        /// <value>The color of the header text.</value>
        public Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        /// <summary>
        /// The header font size property.
        /// </summary>
        public static BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView)bindable)
            );

        /// <summary>
        /// Section header text font size
        /// </summary>
        /// <value>The size of the header font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static BindableProperty HeaderFontFamilyProperty = BindableProperty.Create(
            nameof(HeaderFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string HeaderFontFamily {
            get { return (string)GetValue(HeaderFontFamilyProperty); }
            set { SetValue(HeaderFontFamilyProperty, value); }
        }

        public static BindableProperty HeaderFontAttributesProperty = BindableProperty.Create(
            nameof(HeaderFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes HeaderFontAttributes {
            get { return (FontAttributes)GetValue(HeaderFontAttributesProperty); }
            set { SetValue(HeaderFontAttributesProperty, value); }
        }

        /// <summary>
        /// The header text vertical align property.
        /// </summary>
        public static BindableProperty HeaderTextVerticalAlignProperty =
            BindableProperty.Create(
                nameof(HeaderTextVerticalAlign),
                typeof(LayoutAlignment),
                typeof(SettingsView),
                LayoutAlignment.End,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section header text vertical alignment.
        /// </summary>
        /// <value>The header text vertical align.</value>
        public LayoutAlignment HeaderTextVerticalAlign
        {
            get { return (LayoutAlignment)GetValue(HeaderTextVerticalAlignProperty); }
            set { SetValue(HeaderTextVerticalAlignProperty, value); }
        }

        /// <summary>
        /// The header background color property.
        /// </summary>
        public static BindableProperty HeaderBackgroundColorProperty =
            BindableProperty.Create(
                nameof(HeaderBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section header background color.
        /// </summary>
        /// <value>The color of the header background.</value>
        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        /// <summary>
        /// The header height property.
        /// </summary>
        public static BindableProperty HeaderHeightProperty =
            BindableProperty.Create(
                nameof(HeaderHeight),
                typeof(double),
                typeof(SettingsView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section header height.
        /// </summary>
        /// <value>The height of the header.</value>
        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        /// <summary>
        /// The footer text color property.
        /// </summary>
        public static BindableProperty FooterTextColorProperty =
            BindableProperty.Create(
                nameof(FooterTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section footer text.
        /// </summary>
        /// <value>The color of the footer text.</value>
        public Color FooterTextColor
        {
            get { return (Color)GetValue(FooterTextColorProperty); }
            set { SetValue(FooterTextColorProperty, value); }
        }

        /// <summary>
        /// The footer font size property.
        /// </summary>
        public static BindableProperty FooterFontSizeProperty =
            BindableProperty.Create(
                nameof(FooterFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView)bindable)
            );

        /// <summary>
        /// Section footer text font size.
        /// </summary>
        /// <value>The size of the footer font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double FooterFontSize
        {
            get { return (double)GetValue(FooterFontSizeProperty); }
            set { SetValue(FooterFontSizeProperty, value); }
        }

        public static BindableProperty FooterFontFamilyProperty = BindableProperty.Create(
            nameof(FooterFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string FooterFontFamily {
            get { return (string)GetValue(FooterFontFamilyProperty); }
            set { SetValue(FooterFontFamilyProperty, value); }
        }

        public static BindableProperty FooterFontAttributesProperty = BindableProperty.Create(
            nameof(FooterFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes FooterFontAttributes {
            get { return (FontAttributes)GetValue(FooterFontAttributesProperty); }
            set { SetValue(FooterFontAttributesProperty, value); }
        }

        /// <summary>
        /// The footer background color property.
        /// </summary>
        public static BindableProperty FooterBackgroundColorProperty =
            BindableProperty.Create(
                nameof(FooterBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section footer background color.
        /// </summary>
        /// <value>The color of the footer background.</value>
        public Color FooterBackgroundColor
        {
            get { return (Color)GetValue(FooterBackgroundColorProperty); }
            set { SetValue(FooterBackgroundColorProperty, value); }
        }

        /// <summary>
        /// The footer padding property.
        /// </summary>
        public static BindableProperty FooterPaddingProperty =
            BindableProperty.Create(
                nameof(FooterPadding),
                typeof(Thickness),
                typeof(SettingsView),
                new Thickness(14, 8, 14, 8),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Section footer padding.
        /// </summary>
        /// <value>The footer padding.</value>
        public Thickness FooterPadding
        {
            get { return (Thickness)GetValue(FooterPaddingProperty); }
            set { SetValue(FooterPaddingProperty, value); }
        }

        /// <summary>
        /// The cell title color property.
        /// </summary>
        public static BindableProperty CellTitleColorProperty =
            BindableProperty.Create(
                nameof(CellTitleColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// The color of the cell title.
        /// </summary>
        /// <value>The color of the cell title.</value>
        public Color CellTitleColor
        {
            get { return (Color)GetValue(CellTitleColorProperty); }
            set { SetValue(CellTitleColorProperty, value); }
        }

        /// <summary>
        /// The cell title font size property.
        /// </summary>
        public static BindableProperty CellTitleFontSizeProperty =
            BindableProperty.Create(
                nameof(CellTitleFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (SettingsView)bindable)
            );

        /// <summary>
        /// The font size of the cell title.
        /// </summary>
        /// <value>The size of the cell title font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double CellTitleFontSize
        {
            get { return (double)GetValue(CellTitleFontSizeProperty); }
            set { SetValue(CellTitleFontSizeProperty, value); }
        }

        /// <summary>
        /// The cell title font family.
        /// </summary>
        public static BindableProperty CellTitleFontFamilyProperty = BindableProperty.Create(
            nameof(CellTitleFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );
        /// <summary>
        /// The cell title font family.
        /// </summary>
        public string CellTitleFontFamily {
            get { return (string)GetValue(CellTitleFontFamilyProperty); }
            set { SetValue(CellTitleFontFamilyProperty, value); }
        }

        public static BindableProperty CellTitleFontAttributesProperty = BindableProperty.Create(
            nameof(CellTitleFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes CellTitleFontAttributes {
            get { return (FontAttributes)GetValue(CellTitleFontAttributesProperty); }
            set { SetValue(CellTitleFontAttributesProperty, value); }
        }

        /// <summary>
        /// The cell value text color property.
        /// </summary>
        public static BindableProperty CellValueTextColorProperty =
            BindableProperty.Create(
                nameof(CellValueTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// The color of the cell value text.
        /// </summary>
        /// <value>The color of the cell value text.</value>
        public Color CellValueTextColor
        {
            get { return (Color)GetValue(CellValueTextColorProperty); }
            set { SetValue(CellValueTextColorProperty, value); }
        }

        /// <summary>
        /// The cell value text font size property.
        /// </summary>
        public static BindableProperty CellValueTextFontSizeProperty =
            BindableProperty.Create(
                nameof(CellValueTextFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// The font size of the cell value text.
        /// </summary>
        /// <value>The size of the cell value text font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double CellValueTextFontSize
        {
            get { return (double)GetValue(CellValueTextFontSizeProperty); }
            set { SetValue(CellValueTextFontSizeProperty, value); }
        }

        public static BindableProperty CellValueTextFontFamilyProperty = BindableProperty.Create(
            nameof(CellValueTextFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string CellValueTextFontFamily {
            get { return (string)GetValue(CellValueTextFontFamilyProperty); }
            set { SetValue(CellValueTextFontFamilyProperty, value); }
        }

        public static BindableProperty CellValueTextFontAttributesProperty = BindableProperty.Create(
            nameof(CellValueTextFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes CellValueTextFontAttributes {
            get { return (FontAttributes)GetValue(CellValueTextFontAttributesProperty); }
            set { SetValue(CellValueTextFontAttributesProperty, value); }
        }

        /// <summary>
        /// The cell description color property.
        /// </summary>
        public static BindableProperty CellDescriptionColorProperty =
            BindableProperty.Create(
                nameof(CellDescriptionColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// The color of the cell description text.
        /// </summary>
        /// <value>The color of the cell description.</value>
        public Color CellDescriptionColor
        {
            get { return (Color)GetValue(CellDescriptionColorProperty); }
            set { SetValue(CellDescriptionColorProperty, value); }
        }

        /// <summary>
        /// The cell description font size property.
        /// </summary>
        public static BindableProperty CellDescriptionFontSizeProperty =
            BindableProperty.Create(
                nameof(CellDescriptionFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// The font size of the cell description text.
        /// </summary>
        /// <value>The size of the cell description font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double CellDescriptionFontSize
        {
            get { return (double)GetValue(CellDescriptionFontSizeProperty); }
            set { SetValue(CellDescriptionFontSizeProperty, value); }
        }

        public static BindableProperty CellDescriptionFontFamilyProperty = BindableProperty.Create(
            nameof(CellDescriptionFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string CellDescriptionFontFamily {
            get { return (string)GetValue(CellDescriptionFontFamilyProperty); }
            set { SetValue(CellDescriptionFontFamilyProperty, value); }
        }

        public static BindableProperty CellDescriptionFontAttributesProperty = BindableProperty.Create(
            nameof(CellDescriptionFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes CellDescriptionFontAttributes {
            get { return (FontAttributes)GetValue(CellDescriptionFontAttributesProperty); }
            set { SetValue(CellDescriptionFontAttributesProperty, value); }
        }

        /// <summary>
        /// The cell background color property.
        /// </summary>
        public static BindableProperty CellBackgroundColorProperty =
            BindableProperty.Create(
                nameof(CellBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the cell background.
        /// </summary>
        /// <value>The color of the cell background.</value>
        public Color CellBackgroundColor
        {
            get { return (Color)GetValue(CellBackgroundColorProperty); }
            set { SetValue(CellBackgroundColorProperty, value); }
        }

        /// <summary>
        /// The cell icon size property.
        /// </summary>
        public static BindableProperty CellIconSizeProperty =
            BindableProperty.Create(
                nameof(CellIconSize),
                typeof(Size),
                typeof(SettingsView),
                default(Size),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the size of the cell icon.
        /// </summary>
        /// <value>The size of the cell icon.</value>
        [TypeConverter(typeof(SizeConverter))]
        public Size CellIconSize
        {
            get { return (Size)GetValue(CellIconSizeProperty); }
            set { SetValue(CellIconSizeProperty, value); }
        }

        /// <summary>
        /// The cell icon radius property.
        /// </summary>
        public static BindableProperty CellIconRadiusProperty =
            BindableProperty.Create(
                nameof(CellIconRadius),
                typeof(double),
                typeof(SettingsView),
                6.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the cell icon radius.
        /// </summary>
        /// <value>The cell icon radius.</value>
        public double CellIconRadius
        {
            get { return (double)GetValue(CellIconRadiusProperty); }
            set { SetValue(CellIconRadiusProperty, value); }
        }

        /// <summary>
        /// The cell accent color property.
        /// </summary>
        public static BindableProperty CellAccentColorProperty =
            BindableProperty.Create(
                nameof(CellAccentColor),
                typeof(Color),
                typeof(SettingsView),
                Color.Accent,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the cell accent.
        /// </summary>
        /// <value>The color of the cell accent.</value>
        public Color CellAccentColor
        {
            get { return (Color)GetValue(CellAccentColorProperty); }
            set { SetValue(CellAccentColorProperty, value); }
        }

        /// <summary>
        /// The cell hint text color property.
        /// </summary>
        public static BindableProperty CellHintTextColorProperty =
            BindableProperty.Create(
                nameof(CellHintTextColor),
                typeof(Color),
                typeof(SettingsView),
                Color.Red,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the cell hint text.
        /// </summary>
        /// <value>The color of the cell hint text.</value>
        public Color CellHintTextColor
        {
            get { return (Color)GetValue(CellHintTextColorProperty); }
            set { SetValue(CellHintTextColorProperty, value); }
        }

        /// <summary>
        /// The cell hint font size property.
        /// </summary>
        public static BindableProperty CellHintFontSizeProperty =
            BindableProperty.Create(
                nameof(CellHintFontSize),
                typeof(double),
                typeof(SettingsView),
                10.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the font size of the cell hint text.
        /// </summary>
        /// <value>The size of the cell hint font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double CellHintFontSize
        {
            get { return (double)GetValue(CellHintFontSizeProperty); }
            set { SetValue(CellHintFontSizeProperty, value); }
        }

        public static BindableProperty CellHintFontFamilyProperty = BindableProperty.Create(
            nameof(CellHintFontFamily),
            typeof(string),
            typeof(SettingsView),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string CellHintFontFamily {
            get { return (string)GetValue(CellHintFontFamilyProperty); }
            set { SetValue(CellHintFontFamilyProperty, value); }
        }

        public static BindableProperty CellHintFontAttributesProperty = BindableProperty.Create(
            nameof(CellHintFontAttributes),
            typeof(FontAttributes),
            typeof(SettingsView),
            FontAttributes.None,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes CellHintFontAttributes {
            get { return (FontAttributes)GetValue(CellHintFontAttributesProperty); }
            set { SetValue(CellHintFontAttributesProperty, value); }
        }

        //Only Android 
        /// <summary>
        /// The use description as value property.
        /// </summary>
        public static BindableProperty UseDescriptionAsValueProperty =
            BindableProperty.Create(
                nameof(UseDescriptionAsValue),
                typeof(bool),
                typeof(SettingsView),
                false,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.SettingsView"/> use description
        /// as value. (Only Android)
        /// </summary>
        /// <value><c>true</c> if use description as value; otherwise, <c>false</c>.</value>
        public bool UseDescriptionAsValue
        {
            get { return (bool)GetValue(UseDescriptionAsValueProperty); }
            set { SetValue(UseDescriptionAsValueProperty, value); }
        }

        //Only Android
        /// <summary>
        /// The show section top bottom border property.
        /// </summary>
        public static BindableProperty ShowSectionTopBottomBorderProperty =
            BindableProperty.Create(
                nameof(ShowSectionTopBottomBorder),
                typeof(bool),
                typeof(SettingsView),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.SettingsView"/> show section top
        /// and bottom border. (Only Android)
        /// </summary>
        /// <value><c>true</c> if show section top bottom border; otherwise, <c>false</c>.</value>
        public bool ShowSectionTopBottomBorder
        {
            get { return (bool)GetValue(ShowSectionTopBottomBorderProperty); }
            set { SetValue(ShowSectionTopBottomBorderProperty, value); }
        }

        /// <summary>
        /// The scroll to bottom property.
        /// </summary>
        public static BindableProperty ScrollToBottomProperty =
            BindableProperty.Create(
                nameof(ScrollToBottom),
                typeof(bool),
                typeof(SettingsView),
                default(bool),
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.SettingsView"/> scroll to bottom.
        /// </summary>
        /// <value><c>true</c> if scroll to bottom; otherwise, <c>false</c>.</value>
        public bool ScrollToBottom
        {
            get { return (bool)GetValue(ScrollToBottomProperty); }
            set { SetValue(ScrollToBottomProperty, value); }
        }

        /// <summary>
        /// The scroll to top property.
        /// </summary>
        public static BindableProperty ScrollToTopProperty =
            BindableProperty.Create(
                nameof(ScrollToTop),
                typeof(bool),
                typeof(SettingsView),
                default(bool),
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.SettingsView"/> scroll to top.
        /// </summary>
        /// <value><c>true</c> if scroll to top; otherwise, <c>false</c>.</value>
        public bool ScrollToTop
        {
            get { return (bool)GetValue(ScrollToTopProperty); }
            set { SetValue(ScrollToTopProperty, value); }
        }

        /// <summary>
        /// The computed content height property.
        /// </summary>
        public static BindableProperty VisibleContentHeightProperty =
            BindableProperty.Create(
                nameof(VisibleContentHeight),
                typeof(double),
                typeof(SettingsView),
                -1d,
                defaultBindingMode: BindingMode.OneWayToSource
            );

        /// <summary>
        /// Gets or sets the height of the computed content.
        /// </summary>
        /// <value>The height of the computed content.</value>
        public double VisibleContentHeight {
            get { return (double)GetValue(VisibleContentHeightProperty); }
            set { SetValue(VisibleContentHeightProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(SettingsView),
                default(IEnumerable),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged
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
        /// The item template property.
        /// </summary>
        public static BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(SettingsView),
                default(DataTemplate),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemTemplate {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// The template start index property.
        /// </summary>
        public static BindableProperty TemplateStartIndexProperty =
            BindableProperty.Create(
                nameof(TemplateStartIndex),
                typeof(int),
                typeof(SettingsView),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the index of the template start.
        /// </summary>
        /// <value>The index of the template start.</value>
        public int TemplateStartIndex
        {
            get { return (int)GetValue(TemplateStartIndexProperty); }
            set { SetValue(TemplateStartIndexProperty, value); }
        }

        /// <summary>
        /// The show arrow indicator for android property.
        /// </summary>
        public static BindableProperty ShowArrowIndicatorForAndroidProperty =
            BindableProperty.Create(
                nameof(ShowArrowIndicatorForAndroid),
                typeof(bool),
                typeof(SettingsView),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.SettingsView"/> show arrow
        /// indicator for android.
        /// </summary>
        /// <value><c>true</c> if show arrow indicator for android; otherwise, <c>false</c>.</value>
        public bool ShowArrowIndicatorForAndroid {
            get { return (bool)GetValue(ShowArrowIndicatorForAndroidProperty); }
            set { SetValue(ShowArrowIndicatorForAndroidProperty, value); }
        }

        int templatedItemsCount;

        static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var settingsView = (SettingsView)bindable;

            if (settingsView.ItemTemplate == null) {
                return;
            }

            IList oldValueAsEnumerable;
            IList newValueAsEnumerable;
            try 
            {
                oldValueAsEnumerable = oldValue as IList;
                newValueAsEnumerable = newValue as IList;
            }
            catch (Exception e)
            {
                throw e;
            }


            var oldObservableCollection = oldValue as INotifyCollectionChanged;

            if (oldObservableCollection != null) {
                oldObservableCollection.CollectionChanged -= settingsView.OnItemsSourceCollectionChanged;
            }

            // keep the platform from notifying itemchanged event.
            settingsView.Root.CollectionChanged -= settingsView.OnCollectionChanged;

            if (oldValueAsEnumerable != null)
            {
                for (var i = oldValueAsEnumerable.Count - 1; i >= 0; i--)
                {
                    settingsView.Root.RemoveAt(settingsView.TemplateStartIndex + i);
                }
            }

            if (newValueAsEnumerable != null)
            {
                for (var i = 0; i < newValueAsEnumerable.Count; i++)
                {
                    var view = CreateChildViewFor(settingsView.ItemTemplate, newValueAsEnumerable[i], settingsView);
                    settingsView.Root.Insert(settingsView.TemplateStartIndex + i, view);
                }
                settingsView.templatedItemsCount = newValueAsEnumerable.Count;
            }

            settingsView.Root.CollectionChanged += settingsView.OnCollectionChanged;

            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null) {
                newObservableCollection.CollectionChanged += settingsView.OnItemsSourceCollectionChanged;
            }


            // Notify manually ModelChanged.
            settingsView.OnModelChanged();
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace) {

                //Root.RemoveAt(e.OldStartingIndex + TemplateStartIndex);

                var item = e.NewItems[0];
                var view = CreateChildViewFor(this.ItemTemplate, item, this);

                //Root.Insert(e.NewStartingIndex + TemplateStartIndex, view);
                Root[e.NewStartingIndex + TemplateStartIndex] = view;
            }

            else if (e.Action == NotifyCollectionChangedAction.Add) {
                if (e.NewItems != null) {
                    for (var i = 0; i < e.NewItems.Count; ++i) {
                        var item = e.NewItems[i];
                        var view = CreateChildViewFor(this.ItemTemplate, item, this);

                        Root.Insert(i + e.NewStartingIndex + TemplateStartIndex, view);
                        templatedItemsCount++;
                    }
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove) {
                if (e.OldItems != null) {
                    Root.RemoveAt(e.OldStartingIndex + TemplateStartIndex);
                    templatedItemsCount--;
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Reset) {

                Root.CollectionChanged -= OnCollectionChanged;
                IList source = ItemsSource as IList;
                for (var i = templatedItemsCount - 1; i >= 0; i--)
                {
                    Root.RemoveAt(TemplateStartIndex + i);
                }
                Root.CollectionChanged += OnCollectionChanged;
                templatedItemsCount = 0;
                OnModelChanged();
            }

            else {
                return;
            }

        }

        internal void SendItemDropped(Section section,Cell cell)
        {
            var eventArgs = new DropEventArgs(section, cell);
            ItemDropped?.Invoke(this, eventArgs);
            ItemDroppedCommand?.Execute(eventArgs);
        }

        static Section CreateChildViewFor(DataTemplate template, object item, BindableObject container)
        {
            var selector = template as DataTemplateSelector;

            if (selector != null) {
                template = selector.SelectTemplate(item, container);
            }

            //Binding context
            template.SetValue(BindableObject.BindingContextProperty, item);

            return (Section)template.CreateContent();
        }
    }
}
