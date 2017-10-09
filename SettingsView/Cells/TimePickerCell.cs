using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class TimePickerCell:LabelCell
    {
        public TimePickerCell()
        {
        }

        public static BindableProperty TimeProperty =
            BindableProperty.Create(
                nameof(Time),
                typeof(TimeSpan),
                typeof(TimePickerCell),
                default(TimeSpan),
                defaultBindingMode: BindingMode.TwoWay
            );

        public TimeSpan Time {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static BindableProperty FormatProperty =
            BindableProperty.Create(
                nameof(Format),
                typeof(string),
                typeof(TimePickerCell),
                "t",
                defaultBindingMode: BindingMode.OneWay
            );

        public string Format {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static BindableProperty PickerTitleProperty =
            BindableProperty.Create(
                nameof(PickerTitleProperty),
                typeof(string),
                typeof(TimePickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string PickerTitle {
            get { return (string)GetValue(PickerTitleProperty); }
            set { SetValue(PickerTitleProperty, value); }
        }
    }
}
