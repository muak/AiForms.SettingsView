using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AiForms.Renderers;

namespace Sample.Views.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCellA : CustomCell
    {
        public MyCellA()
        {
            InitializeComponent();
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(MyCellA),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
