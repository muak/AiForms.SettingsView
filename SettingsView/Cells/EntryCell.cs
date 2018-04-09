using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Entry cell.
    /// </summary>
    public class EntryCell:CellBase,IEntryCellController
    {
        /// <summary>
        /// The value text property.
        /// </summary>
        public static BindableProperty ValueTextProperty =
            BindableProperty.Create(
                nameof(ValueText),
                typeof(string),
                typeof(EntryCell),
                default(string),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanging:ValueTextPropertyChanging

            );

        static void  ValueTextPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var maxlength = (int)bindable.GetValue(MaxLengthProperty);
            if (maxlength < 0) return;

            var newString = newValue?.ToString() ?? string.Empty;
            if (newString.Length > maxlength) {
                var oldString = oldValue?.ToString() ?? string.Empty;
                if(oldString.Length > maxlength){
                    var trimStr = oldString.Substring(0, maxlength);
                    bindable.SetValue(ValueTextProperty, trimStr);
                }
                else{
                    bindable.SetValue(ValueTextProperty, oldString);
                }
               
            }
        }

        /// <summary>
        /// Gets or sets the value text.
        /// </summary>
        /// <value>The value text.</value>
        public string ValueText {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
        }

        /// <summary>
        /// The max length property.
        /// </summary>
        public static BindableProperty MaxLengthProperty =
            BindableProperty.Create(
                nameof(MaxLength),
                typeof(int),
                typeof(EntryCell),
                -1,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the length of the max.
        /// </summary>
        /// <value>The length of the max.</value>
        public int MaxLength {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// The value text color property.
        /// </summary>
        public static BindableProperty ValueTextColorProperty =
            BindableProperty.Create(
                nameof(ValueTextColor),
                typeof(Color),
                typeof(EntryCell),
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
                typeof(EntryCell),
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

        /// <summary>
        /// The keyboard property.
        /// </summary>
        public static BindableProperty KeyboardProperty =
            BindableProperty.Create(
                nameof(Keyboard),
                typeof(Keyboard),
                typeof(EntryCell),
                Keyboard.Default,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the keyboard.
        /// </summary>
        /// <value>The keyboard.</value>
        public Keyboard Keyboard {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        /// <summary>
        /// Occurs when completed.
        /// </summary>
        public event EventHandler Completed;
        /// <summary>
        /// Sends the completed.
        /// </summary>
        public void SendCompleted() {
           EventHandler handler = Completed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(
                nameof(Placeholder),
                typeof(string),
                typeof(EntryCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>The placeholder.</value>
        public string Placeholder {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        /// <summary>
        /// The text alignment property.
        /// </summary>
        public static BindableProperty TextAlignmentProperty =
            BindableProperty.Create(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(EntryCell),
                TextAlignment.End,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// The accent color property.
        /// </summary>
        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(EntryCell),
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
    }
}
