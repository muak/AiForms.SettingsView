using System;
using AiForms.Renderers;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class RadioCell:CellBase
    {
        /// <summary>
        /// The selected value property.
        /// </summary>
        public static readonly BindableProperty SelectedValueProperty =
            BindableProperty.CreateAttached(
                    "SelectedValue",
                    typeof(object),
                    typeof(RadioCell),
                    default(object),
                    BindingMode.TwoWay
                );

        /// <summary>
        /// Sets the selected value.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetSelectedValue(BindableObject view, object value)
        {
            view.SetValue(SelectedValueProperty, value);
        }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <returns>The selected value.</returns>
        /// <param name="view">View.</param>
        public static object GetSelectedValue(BindableObject view)
        {
            return (object)view.GetValue(SelectedValueProperty);
        }

        /// <summary>
        /// The value property.
        /// </summary>
        public static BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(object),
                typeof(RadioCell),
                default(object),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// The accent color property.
        /// </summary>
        public static BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Color),
                typeof(RadioCell),
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
