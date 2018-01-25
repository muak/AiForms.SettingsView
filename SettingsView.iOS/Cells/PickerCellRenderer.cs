using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Picker cell renderer.
    /// </summary>
    public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

    /// <summary>
    /// Picker cell view.
    /// </summary>
    public class PickerCellView : LabelCellView
    {
        PickerCell _PickerCell => Cell as PickerCell;
        string _valueTextCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.PickerCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public PickerCellView(Cell formsCell) : base(formsCell)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
            SelectionStyle = UITableViewCellSelectionStyle.Default;
            SetRightMarginZero();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
                e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
                e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
                e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName) {
                UpdateSelectedItems(true);
            }
            if(e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName){
                if(_PickerCell.UseAutoValueText){
                    UpdateSelectedItems(true);
                }
                else{
                    base.UpdateValueText();
                }
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateSelectedItems();
        }

        /// <summary>
        /// Updates the selected items.
        /// </summary>
        /// <param name="force">If set to <c>true</c> force.</param>
        public void UpdateSelectedItems(bool force = false)
        {
            if(!_PickerCell.UseAutoValueText){
                return;
            }

            if (force || string.IsNullOrEmpty(_valueTextCache)) {
                _valueTextCache = _PickerCell.GetSelectedItemsText();
            }

            ValueLabel.Text = _valueTextCache;
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {

            }
            base.Dispose(disposing);
        }

    }
}
