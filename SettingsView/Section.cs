using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class Section : TableSectionBase<Cell>
    {
        public Section()
        {
        }

        public Section(string title) : base(title)
        {

        }

        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(Section),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty FooterTextProperty =
            BindableProperty.Create(
                nameof(FooterText),
                typeof(string),
                typeof(Section),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string FooterText
        {
            get { return (string)GetValue(FooterTextProperty); }
            set { SetValue(FooterTextProperty, value); }
        }
    }
}
