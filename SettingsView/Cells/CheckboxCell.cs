using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class CheckboxCell:CellBase
    {
        public CheckboxCell()
        {
        }

        public static BindableProperty CheckedProperty =
            BindableProperty.Create(
                nameof(Checked),
                typeof(bool),
                typeof(CheckboxCell),
                default(bool),
                defaultBindingMode: BindingMode.TwoWay
            );

        public bool Checked {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(CheckboxCell),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color AccentColor {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
    }
}
