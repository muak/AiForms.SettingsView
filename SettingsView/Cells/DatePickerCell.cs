using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class DatePickerCell:LabelCell
    {
        public DatePickerCell()
        {
        }

        public static BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(DatePickerCell),
                default(DateTime),
                defaultBindingMode: BindingMode.TwoWay
            );

        public DateTime Date {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static BindableProperty MaximumDateProperty =
            BindableProperty.Create(
                nameof(MaximumDate),
                typeof(DateTime),
                typeof(DatePickerCell),
                new DateTime(2100, 12, 31),
                defaultBindingMode: BindingMode.OneWay
            );

        public DateTime MaximumDate {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public static BindableProperty MinimumDateProperty =
            BindableProperty.Create(
                nameof(MinimumDate),
                typeof(DateTime),
                typeof(DatePickerCell),
                new DateTime(1900, 1, 1),
                defaultBindingMode: BindingMode.OneWay
            );

        public DateTime MinimumDate {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public static BindableProperty FormatProperty =
            BindableProperty.Create(
                nameof(Format),
                typeof(string),
                typeof(DatePickerCell),
                "d",
                defaultBindingMode: BindingMode.OneWay
            );

        public string Format {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static BindableProperty TodayTextProperty =
            BindableProperty.Create(
                nameof(TodayText),
                typeof(string),
                typeof(DatePickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string TodayText {
            get { return (string)GetValue(TodayTextProperty); }
            set { SetValue(TodayTextProperty, value); }
        }

        private new string ValueText { get; set; }
    }
}
