using System;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.iOS
{                 
    public class PickerCellRenderer:CellBaseRenderer<PickerCellView>{}

    public class PickerCellView : LabelCellView
    {
        PickerCell _PickerCell => Cell as PickerCell;

        public PickerCellView(Cell formsCell) : base(formsCell)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            SelectionStyle = UITableViewCellSelectionStyle.Default;
            SetRightMarginZero();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if(e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName){
                
            }
            else if(e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName){
                
            }
            else if(e.PropertyName == PickerCell.KeepSelectedUntilBackProperty.PropertyName){
                
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                
            }
            base.Dispose(disposing);
        }


    }
}
