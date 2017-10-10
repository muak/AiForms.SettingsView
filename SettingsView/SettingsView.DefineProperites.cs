using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public partial class SettingsView
    {
        public static new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public new Color BackgroundColor {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static BindableProperty SeparatorColorProperty =
            BindableProperty.Create(
                nameof(SeparatorColor),
                typeof(Color),
                typeof(SettingsView),
                Color.FromRgb(199, 199, 204),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color SeparatorColor {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public static BindableProperty HeaderPaddingProperty =
            BindableProperty.Create(
                nameof(HeaderPadding),
                typeof(Thickness),
                typeof(SettingsView),
                new Thickness(14,8,8,8),
                defaultBindingMode: BindingMode.OneWay
            );

        public Thickness HeaderPadding {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        public static BindableProperty HeaderTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color HeaderTextColor {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        public static BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView)bindable)
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double HeaderFontSize {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static BindableProperty HeaderTextVerticalAlignProperty =
            BindableProperty.Create(
                nameof(HeaderTextVerticalAlign),
                typeof(LayoutAlignment),
                typeof(SettingsView),
                LayoutAlignment.End,
                defaultBindingMode: BindingMode.OneWay
            );

        public LayoutAlignment HeaderTextVerticalAlign {
            get { return (LayoutAlignment)GetValue(HeaderTextVerticalAlignProperty); }
            set { SetValue(HeaderTextVerticalAlignProperty, value); }
        }

        public static BindableProperty HeaderBackgroundColorProperty =
            BindableProperty.Create(
                nameof(HeaderBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color HeaderBackgroundColor {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        public static BindableProperty HeaderHeightProperty =
            BindableProperty.Create(
                nameof(HeaderHeight),
                typeof(double),
                typeof(SettingsView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double HeaderHeight {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static BindableProperty FooterTextColorProperty =
            BindableProperty.Create(
                nameof(FooterTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color FooterTextColor {
            get { return (Color)GetValue(FooterTextColorProperty); }
            set { SetValue(FooterTextColorProperty, value); }
        }

        public static BindableProperty FooterFontSizeProperty =
            BindableProperty.Create(
                nameof(FooterFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView)bindable)
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double FooterFontSize {
            get { return (double)GetValue(FooterFontSizeProperty); }
            set { SetValue(FooterFontSizeProperty, value); }
        }

        public static BindableProperty FooterBackgroundColorProperty =
            BindableProperty.Create(
                nameof(FooterBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color FooterBackgroundColor {
            get { return (Color)GetValue(FooterBackgroundColorProperty); }
            set { SetValue(FooterBackgroundColorProperty, value); }
        }

        public static BindableProperty FooterPaddingProperty =
            BindableProperty.Create(
                nameof(FooterPadding),
                typeof(Thickness),
                typeof(SettingsView),
                new Thickness(14,8,14,8),
                defaultBindingMode: BindingMode.OneWay
            );

        public Thickness FooterPadding {
            get { return (Thickness)GetValue(FooterPaddingProperty); }
            set { SetValue(FooterPaddingProperty, value); }
        }


        public static BindableProperty CellTitleColorProperty =
            BindableProperty.Create(
                nameof(CellTitleColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellTitleColor {
            get { return (Color)GetValue(CellTitleColorProperty); }
            set { SetValue(CellTitleColorProperty, value); }
        }


        public static BindableProperty CellTitleFontSizeProperty =
            BindableProperty.Create(
                nameof(CellTitleFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0,
                defaultBindingMode: BindingMode.OneWay,
                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (SettingsView)bindable)
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double CellTitleFontSize {
            get { return (double)GetValue(CellTitleFontSizeProperty); }
            set { SetValue(CellTitleFontSizeProperty, value); }
        }

        public static BindableProperty CellValueTextColorProperty =
            BindableProperty.Create(
                nameof(CellValueTextColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellValueTextColor {
            get { return (Color)GetValue(CellValueTextColorProperty); }
            set { SetValue(CellValueTextColorProperty, value); }
        }

        public static BindableProperty CellValueTextFontSizeProperty =
            BindableProperty.Create(
                nameof(CellValueTextFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double CellValueTextFontSize {
            get { return (double)GetValue(CellValueTextFontSizeProperty); }
            set { SetValue(CellValueTextFontSizeProperty, value); }
        }

        public static BindableProperty CellDescriptionColorProperty =
            BindableProperty.Create(
                nameof(CellDescriptionColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellDescriptionColor {
            get { return (Color)GetValue(CellDescriptionColorProperty); }
            set { SetValue(CellDescriptionColorProperty, value); }
        }

        public static BindableProperty CellDescriptionFontSizeProperty =
            BindableProperty.Create(
                nameof(CellDescriptionFontSize),
                typeof(double),
                typeof(SettingsView),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double CellDescriptionFontSize {
            get { return (double)GetValue(CellDescriptionFontSizeProperty); }
            set { SetValue(CellDescriptionFontSizeProperty, value); }
        }

        public static BindableProperty CellBackgroundColorProperty =
            BindableProperty.Create(
                nameof(CellBackgroundColor),
                typeof(Color),
                typeof(SettingsView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellBackgroundColor {
            get { return (Color)GetValue(CellBackgroundColorProperty); }
            set { SetValue(CellBackgroundColorProperty, value); }
        }

        public static BindableProperty CellIconSizeProperty =
            BindableProperty.Create(
                nameof(CellIconSize),
                typeof(Size),
                typeof(SettingsView),
                default(Size),
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(SizeConverter))]
        public Size CellIconSize {
            get { return (Size)GetValue(CellIconSizeProperty); }
            set { SetValue(CellIconSizeProperty, value); }
        }

        public static BindableProperty CellAccentColorProperty =
            BindableProperty.Create(
                nameof(CellAccentColor),
                typeof(Color),
                typeof(SettingsView),
                Color.Accent,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellAccentColor {
            get { return (Color)GetValue(CellAccentColorProperty); }
            set { SetValue(CellAccentColorProperty, value); }
        }

        public static BindableProperty CellHintTextColorProperty =
            BindableProperty.Create(
                nameof(CellHintTextColor),
                typeof(Color),
                typeof(SettingsView),
                Color.Red,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color CellHintTextColor
        {
            get { return (Color)GetValue(CellHintTextColorProperty); }
            set { SetValue(CellHintTextColorProperty, value); }
        }

        public static BindableProperty CellHintFontSizeProperty =
            BindableProperty.Create(
                nameof(CellHintFontSize),
                typeof(double),
                typeof(SettingsView),
                10.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double CellHintFontSize
        {
            get { return (double)GetValue(CellHintFontSizeProperty); }
            set { SetValue(CellHintFontSizeProperty, value); }
        }

        //Android Only TODO:未実装
        public static BindableProperty ShowSectionTopBottomBorderProperty =
            BindableProperty.Create(
                nameof(ShowSectionTopBottomBorder),
                typeof(bool),
                typeof(SettingsView),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool ShowSectionTopBottomBorder {
            get { return (bool)GetValue(ShowSectionTopBottomBorderProperty); }
            set { SetValue(ShowSectionTopBottomBorderProperty, value); }
        }
    }
}
