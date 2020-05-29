using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections;

namespace AiForms.Renderers
{
    /// <summary>
    /// Text picker cell.
    /// </summary>
    public class TextPickerCell:LabelCell
    {
        /// <summary>
        /// The items property.
        /// </summary>
        public static BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(IList),
                typeof(TextPickerCell),
                new List<object>(),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList Items {
            get { return (IList)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// The page title property.
        /// </summary>
        public static BindableProperty PageTitleProperty =
            BindableProperty.Create(
                nameof(PageTitle),
                typeof(string),
                typeof(PickerCell),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        /// <summary>
        /// The accent color property.
        /// </summary>
        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(PickerCell),
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

        /// <summary>
        /// The selected item property.
        /// </summary>
        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(TextPickerCell),
                default(object),
                defaultBindingMode: BindingMode.TwoWay
            );

        /// <summary>
        /// Gets or sets the selected number.
        /// </summary>
        /// <value>The text value.</value>
        public object SelectedItem {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// The picker title property.
        /// </summary>
        public static BindableProperty PickerTitleProperty =
            BindableProperty.Create(
                nameof(PickerTitle),
                typeof(string),
                typeof(TextPickerCell),
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
                typeof(TextPickerCell),
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

        /// <summary>
        /// Whether the picker is circular. (only Android)
        /// </summary>
        public static BindableProperty IsCircularPickerProperty = BindableProperty.Create(
            nameof(IsCircularPicker),
            typeof(bool),
            typeof(TextPickerCell),
            true,
            defaultBindingMode: BindingMode.OneWay
        );
        /// <summary>
        /// Whether the picker is circular. (only Android)
        /// </summary>
        public bool IsCircularPicker {
            get { return (bool)GetValue(IsCircularPickerProperty); }
            set { SetValue(IsCircularPickerProperty, value); }
        }

        private new string ValueText { get; set; }
    }
}
