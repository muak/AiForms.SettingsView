using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class EntryCell:CellBase,IEntryCellController
    {
        public EntryCell() {
        }

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

            var newString = newValue.ToString();
            if (newString.Length > maxlength) {
                bindable.SetValue(ValueTextProperty,oldValue);
            }
        }

        public string ValueText {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
        }

        public static BindableProperty MaxLengthProperty =
            BindableProperty.Create(
                nameof(MaxLength),
                typeof(int),
                typeof(EntryCell),
                -1,
                defaultBindingMode: BindingMode.OneWay
            );

        public int MaxLength {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static BindableProperty ValueTextColorProperty =
            BindableProperty.Create(
                nameof(ValueTextColor),
                typeof(Color),
                typeof(EntryCell),
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
                typeof(EntryCell),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double ValueTextFontSize {
            get { return (double)GetValue(ValueTextFontSizeProperty); }
            set { SetValue(ValueTextFontSizeProperty, value); }
        }

        public static BindableProperty KeyboardProperty =
            BindableProperty.Create(
                nameof(Keyboard),
                typeof(Keyboard),
                typeof(EntryCell),
                Keyboard.Default,
                defaultBindingMode: BindingMode.OneWay
            );

        public Keyboard Keyboard {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public event EventHandler Completed;
        public void SendCompleted() {
           EventHandler handler = Completed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(
                nameof(Placeholder),
                typeof(string),
                typeof(EntryCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string Placeholder {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(EntryCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color AccentColor {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
    }
}
