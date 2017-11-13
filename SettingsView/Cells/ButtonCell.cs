using System;
using Xamarin.Forms;
using System.Windows.Input;
namespace AiForms.Renderers
{
    /// <summary>
    /// Button cell.
    /// </summary>
    public class ButtonCell:CellBase
    {

        private new string Description { get; set; }
        private new Color DescriptionColor { get; set; }
        private new double DescriptionFontSize { get; set; }

        /// <summary>
        /// The title alignment property.
        /// </summary>
        public static BindableProperty TitleAlignmentProperty =
            BindableProperty.Create(
                nameof(TitleAlignment),
                typeof(TextAlignment),
                typeof(ButtonCell),
                TextAlignment.Center,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the title alignment.
        /// </summary>
        /// <value>The title alignment.</value>
        public TextAlignment TitleAlignment
        {
            get { return (TextAlignment)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
        }

        /// <summary>
        /// The command property.
        /// </summary>
        public static BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(ButtonCell),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// The command parameter property.
        /// </summary>
        public static BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(ButtonCell),
                default(object),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
