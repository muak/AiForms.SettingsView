using System;
using Xamarin.Forms;
using System.Windows.Input;
namespace AiForms.Renderers
{
    public class ButtonCell:CellBase
    {
        public ButtonCell()
        {
        }

        private new string Description { get; set; }
        private new Color DescriptionColor { get; set; }
        private new double DescriptionFontSize { get; set; }

        public static BindableProperty TitleAlignmentProperty =
            BindableProperty.Create(
                nameof(TitleAlignment),
                typeof(TextAlignment),
                typeof(ButtonCell),
                default(TextAlignment),
                defaultBindingMode: BindingMode.OneWay
            );

        public TextAlignment TitleAlignment
        {
            get { return (TextAlignment)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
        }

        public static BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(ButtonCell),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(ButtonCell),
                default(object),
                defaultBindingMode: BindingMode.OneWay
            );

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
