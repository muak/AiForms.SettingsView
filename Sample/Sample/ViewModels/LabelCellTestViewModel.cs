using System;
using Xamarin.Forms;
namespace Sample.ViewModels
{
    public class LabelCellTestViewModel:ViewModelBase
    {
        public LabelCellTestViewModel()
        {
            BackgroundColor.Value = Color.White;
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);
        }
    }
}
