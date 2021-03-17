using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Renderers
{
    /// <summary>
    /// Number picker cell.
    /// </summary>
    public class NumberPickerCell:LabelCell
    {
        /// <summary>
        /// The number property.
        /// </summary>
        public static BindableProperty NumberProperty =
            BindableProperty.Create(
                nameof(Number),
                typeof(int?),
                typeof(NumberPickerCell),
                null,
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public int? Number {
            get { return (int?)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        /// <summary>
        /// The minimum property.
        /// </summary>
        public static BindableProperty MinProperty =
            BindableProperty.Create(
                nameof(Min),
                typeof(int),
                typeof(NumberPickerCell),
                0,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public int Min {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        /// <summary>
        /// The max property.
        /// </summary>
        public static BindableProperty MaxProperty =
            BindableProperty.Create(
                nameof(Max),
                typeof(int),
                typeof(NumberPickerCell),
                9999,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>The max.</value>
        public int Max {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        /// <summary>
        /// The picker title property.
        /// </summary>
        public static BindableProperty PickerTitleProperty =
            BindableProperty.Create(
                nameof(PickerTitle),
                typeof(string),
                typeof(NumberPickerCell),
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

        /// <summary>
        /// The selected command property.
        /// </summary>
        public static BindableProperty SelectedCommandProperty =
            BindableProperty.Create(
                nameof(SelectedCommand),
                typeof(ICommand),
                typeof(NumberPickerCell),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the selected command.
        /// </summary>
        /// <value>The selected command.</value>
        public ICommand SelectedCommand {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        private new string ValueText { get; set; }
    }
}
