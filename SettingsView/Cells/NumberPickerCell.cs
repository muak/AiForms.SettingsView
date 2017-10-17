using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Renderers
{
    public class NumberPickerCell:LabelCell
    {

        public static BindableProperty NumberProperty =
            BindableProperty.Create(
                nameof(Number),
                typeof(int),
                typeof(NumberPickerCell),
                default(int),
                defaultBindingMode: BindingMode.TwoWay
            );

        public int Number {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static BindableProperty MinProperty =
            BindableProperty.Create(
                nameof(Min),
                typeof(int),
                typeof(NumberPickerCell),
                0,
                defaultBindingMode: BindingMode.OneWay
            );

        public int Min {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public static BindableProperty MaxProperty =
            BindableProperty.Create(
                nameof(Max),
                typeof(int),
                typeof(NumberPickerCell),
                9999,
                defaultBindingMode: BindingMode.OneWay
            );

        public int Max {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public static BindableProperty PickerTitleProperty =
            BindableProperty.Create(
                nameof(PickerTitle),
                typeof(string),
                typeof(NumberPickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string PickerTitle {
            get { return (string)GetValue(PickerTitleProperty); }
            set { SetValue(PickerTitleProperty, value); }
        }

        public static BindableProperty SelectedCommandProperty =
            BindableProperty.Create(
                nameof(SelectedCommand),
                typeof(ICommand),
                typeof(NumberPickerCell),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand SelectedCommand {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        private new string ValueText { get; set; }
    }
}
