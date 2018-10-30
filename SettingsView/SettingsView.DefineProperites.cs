using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public partial class SettingsView
    {
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
        public static BindableProperty ComputedContentHeightProperty =
            BindableProperty.Create(
                nameof(ComputedContentHeight),
                typeof(double),
                typeof(SettingsView),
                -1d,
                defaultBindingMode: BindingMode.OneWayToSource
            );

        /// <summary>
        /// Gets or sets the height of the computed content.
        /// </summary>
        /// <value>The height of the computed content.</value>
        public double ComputedContentHeight {
            get { return (double)GetValue(ComputedContentHeightProperty); }
            set { SetValue(ComputedContentHeightProperty, value); }
        }
    }
}
