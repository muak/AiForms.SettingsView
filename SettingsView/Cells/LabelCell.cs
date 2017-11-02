using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class LabelCell:CellBase
    {
        public LabelCell() {
        }

        public static BindableProperty ValueTextProperty =
            BindableProperty.Create(
                nameof(ValueText),
                typeof(string),
                typeof(LabelCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string ValueText {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
        }

        public static BindableProperty ValueTextColorProperty =
            BindableProperty.Create(
                nameof(ValueTextColor),
                typeof(Color),
                typeof(LabelCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color ValueTextColor {
            get { return (Color)GetValue(ValueTextColorProperty); }
            set { SetValue(ValueTextColorProperty, value); }
        }

        public static BindableProperty ValueTextFontSizeProperty =
            BindableProperty.Create(
                nameof(ValueTextFontSize),
                typeof(double),
                typeof(LabelCell),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double ValueTextFontSize {
            get { return (double)GetValue(ValueTextFontSizeProperty); }
            set { SetValue(ValueTextFontSizeProperty, value); }
        }

        public static BindableProperty IgnoreUseDescriptionAsValueProperty =
            BindableProperty.Create(
                nameof(IgnoreUseDescriptionAsValue),
                typeof(bool),
                typeof(LabelCell),
                false,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool IgnoreUseDescriptionAsValue
        {
            get { return (bool)GetValue(IgnoreUseDescriptionAsValueProperty); }
            set { SetValue(IgnoreUseDescriptionAsValueProperty, value); }
        }
    }
}
