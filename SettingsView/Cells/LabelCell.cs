using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Label cell.
    /// </summary>
    public class LabelCell:CellBase
    {
        /// <summary>
        /// The value text property.
        /// </summary>
        public static BindableProperty ValueTextProperty =
            BindableProperty.Create(
                nameof(ValueText),
                typeof(string),
                typeof(LabelCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the value text.
        /// </summary>
        /// <value>The value text.</value>
        public string ValueText {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
        }

        /// <summary>
        /// The value text color property.
        /// </summary>
        public static BindableProperty ValueTextColorProperty =
            BindableProperty.Create(
                nameof(ValueTextColor),
                typeof(Color),
                typeof(LabelCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the value text.
        /// </summary>
        /// <value>The color of the value text.</value>
        public Color ValueTextColor {
            get { return (Color)GetValue(ValueTextColorProperty); }
            set { SetValue(ValueTextColorProperty, value); }
        }

        /// <summary>
        /// The value text font size property.
        /// </summary>
        public static BindableProperty ValueTextFontSizeProperty =
            BindableProperty.Create(
                nameof(ValueTextFontSize),
                typeof(double),
                typeof(LabelCell),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the size of the value text font.
        /// </summary>
        /// <value>The size of the value text font.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double ValueTextFontSize {
            get { return (double)GetValue(ValueTextFontSizeProperty); }
            set { SetValue(ValueTextFontSizeProperty, value); }
        }

        public static BindableProperty ValueTextFontFamilyProperty = BindableProperty.Create(
            nameof(ValueTextFontFamily),
            typeof(string),
            typeof(LabelCell),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string ValueTextFontFamily {
            get { return (string)GetValue(ValueTextFontFamilyProperty); }
            set { SetValue(ValueTextFontFamilyProperty, value); }
        }

        public static BindableProperty ValueTextFontAttributesProperty = BindableProperty.Create(
            nameof(ValueTextFontAttributes),
            typeof(FontAttributes?),
            typeof(LabelCell),
            null,
            defaultBindingMode: BindingMode.OneWay
        );

        public FontAttributes? ValueTextFontAttributes {
            get { return (FontAttributes?)GetValue(ValueTextFontAttributesProperty); }
            set { SetValue(ValueTextFontAttributesProperty, value); }
        }

        /// <summary>
        /// The ignore use description as value property.
        /// </summary>
        public static BindableProperty IgnoreUseDescriptionAsValueProperty =
            BindableProperty.Create(
                nameof(IgnoreUseDescriptionAsValue),
                typeof(bool),
                typeof(LabelCell),
                false,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.LabelCell"/> ignore use
        /// description as value.
        /// </summary>
        /// <value><c>true</c> if ignore use description as value; otherwise, <c>false</c>.</value>
        public bool IgnoreUseDescriptionAsValue
        {
            get { return (bool)GetValue(IgnoreUseDescriptionAsValueProperty); }
            set { SetValue(IgnoreUseDescriptionAsValueProperty, value); }
        }
    }
}
