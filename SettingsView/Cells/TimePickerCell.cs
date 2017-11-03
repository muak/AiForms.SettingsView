using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Time picker cell.
    /// </summary>
    public class TimePickerCell:LabelCell
    {
        /// <summary>
        /// The time property.
        /// </summary>
        public static BindableProperty TimeProperty =
            BindableProperty.Create(
                nameof(Time),
                typeof(TimeSpan),
                typeof(TimePickerCell),
                default(TimeSpan),
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public TimeSpan Time {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        /// <summary>
        /// The format property.
        /// </summary>
        public static BindableProperty FormatProperty =
            BindableProperty.Create(
                nameof(Format),
                typeof(string),
                typeof(TimePickerCell),
                "t",
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public string Format {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        /// <summary>
        /// The picker title property.
        /// </summary>
        public static BindableProperty PickerTitleProperty =
            BindableProperty.Create(
                nameof(PickerTitleProperty),
                typeof(string),
                typeof(TimePickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the picker title.
        /// </summary>
        /// <value>The picker title.</value>
        public string PickerTitle {
            get { return (string)GetValue(PickerTitleProperty); }
            set { SetValue(PickerTitleProperty, value); }
        }

        private new string ValueText { get; set; }
    }
}
