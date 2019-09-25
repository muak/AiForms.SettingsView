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
    public partial class MyCellB : CustomCell
    {
        public MyCellB()
        {
            InitializeComponent();
        }
    }
}
