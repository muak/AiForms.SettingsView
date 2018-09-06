using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;
using System.Collections.Specialized;
using System;

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
        INotifyCollectionChanged _notifyCollection;
        INotifyCollectionChanged _selectedCollection;

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
            if(e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName){
                UpdateCollectionChanged();
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateSelectedItems();
            UpdateCollectionChanged();
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
                if (_selectedCollection != null) {
                    _selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
                }
                _selectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;
                if (_selectedCollection != null) {
                    _selectedCollection.CollectionChanged += SelectedItems_CollectionChanged;
                }
                _valueTextCache = _PickerCell.GetSelectedItemsText();
            }

            ValueLabel.Text = _valueTextCache;
        }

        void UpdateCollectionChanged()
        {
            if(_notifyCollection != null){
                _notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
            }

            _notifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

            if (_notifyCollection != null)
            {
                _notifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
                ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Updates the is enabled.
        /// </summary>
        protected override void UpdateIsEnabled()
        {
            if (_PickerCell.ItemsSource != null && _PickerCell.ItemsSource.Count == 0) {
                return;
            }
            base.UpdateIsEnabled();
        }

        void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!CellBase.IsEnabled){
                return;
            }

            SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
        }

        void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSelectedItems(true);
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                
                if (_notifyCollection != null)
                {
                    _notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
                    _notifyCollection = null;
                }
                if (_selectedCollection != null)
                {
                    _selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
                    _selectedCollection = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}
