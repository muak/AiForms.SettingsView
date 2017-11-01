using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

    public class PickerCellView : LabelCellView
    {
        PickerCell _PickerCell => Cell as PickerCell;
        string _valueTextCache;

        public PickerCellView(Cell formsCell) : base(formsCell)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            SelectionStyle = UITableViewCellSelectionStyle.Default;
            SetRightMarginZero();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
                e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
                e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName) {
                UpdateSelectedItems(true);
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateSelectedItems();
        }

        public void UpdateSelectedItems(bool force = false)
        {
            if (force || string.IsNullOrEmpty(_valueTextCache)) {
                _valueTextCache = _PickerCell.GetSelectedItemsText();
            }

            ValueLabel.Text = _valueTextCache;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {

            }
            base.Dispose(disposing);
        }

    }
}
