using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    internal class PickerTableViewController : UITableViewController
    {

        PickerCell _pickerCell;
        PickerCellView _pickerCellNative;
        SettingsView _parent;
        IList _source;
        Dictionary<int, object> _selectedCache = new Dictionary<int, object>();
        UIColor _accentColor;
        UIColor _titleColor;
        UIColor _detailColor;
        nfloat _fontSize;
        nfloat _detailFontSize;
        UIColor _background;
        UITableView _tableView;

        internal PickerTableViewController(PickerCellView pickerCellView, UITableView tableView):base(UITableViewStyle.Grouped)
        {
            _pickerCell = pickerCellView.Cell as PickerCell;
            _pickerCellNative = pickerCellView;
            _parent = pickerCellView.CellParent;
            _source = _pickerCell.ItemsSource as IList;
            _tableView = tableView;

            if (_pickerCell.SelectedItems == null) {
                _pickerCell.SelectedItems = new List<object>();
            }

            SetUpProperties();
        }


        void SetUpProperties()
        {
            if (_pickerCell.AccentColor != Xamarin.Forms.Color.Default) {
                _accentColor = _pickerCell.AccentColor.ToUIColor();
            }
            else if (_parent.CellAccentColor != Xamarin.Forms.Color.Default) {
                _accentColor = _parent.CellAccentColor.ToUIColor();
            }

            if (_pickerCell.TitleColor != Xamarin.Forms.Color.Default) {
                _titleColor = _pickerCell.TitleColor.ToUIColor();
            }
            else if (_parent != null && _parent.CellTitleColor != Xamarin.Forms.Color.Default) {
                _titleColor = _parent.CellTitleColor.ToUIColor();
            }

            if (_pickerCell.TitleFontSize > 0) {
                _fontSize = (nfloat)_pickerCell.TitleFontSize;
            }
            else if (_parent != null) {
                _fontSize = (nfloat)_parent.CellTitleFontSize;
            }

            if (_pickerCell.DescriptionColor != Xamarin.Forms.Color.Default)
            {
                _detailColor = _pickerCell.DescriptionColor.ToUIColor();
            }
            else if (_parent != null && _parent.CellDescriptionColor != Xamarin.Forms.Color.Default)
            {
                _detailColor = _parent.CellDescriptionColor.ToUIColor();
            }

            if (_pickerCell.DescriptionFontSize > 0)
            {
                _detailFontSize = (nfloat)_pickerCell.DescriptionFontSize;
            }
            else if (_parent != null)
            {
                _detailFontSize = (nfloat)_parent.CellDescriptionFontSize;
            }

            if (_pickerCell.BackgroundColor != Xamarin.Forms.Color.Default) {
                _background = _pickerCell.BackgroundColor.ToUIColor();
            }
            else if (_parent != null && _parent.CellBackgroundColor != Xamarin.Forms.Color.Default) {
                _background = _parent.CellBackgroundColor.ToUIColor();
            }
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {

            var reusableCell = tableView.DequeueReusableCell("pikcercell");
            if (reusableCell == null) {
                reusableCell = new UITableViewCell(UITableViewCellStyle.Subtitle, "pickercell");

                reusableCell.TextLabel.TextColor = _titleColor;
                reusableCell.TextLabel.Font = reusableCell.TextLabel.Font.WithSize(_fontSize);
                reusableCell.DetailTextLabel.TextColor = _detailColor;
                reusableCell.DetailTextLabel.Font = reusableCell.DetailTextLabel.Font.WithSize(_detailFontSize);
                reusableCell.BackgroundColor = _background;
                reusableCell.TintColor = _accentColor;
            }

            var text = _pickerCell.DisplayValue(_source[indexPath.Row]);
            reusableCell.TextLabel.Text = $"{text}";
            var detail = _pickerCell.SubDisplayValue(_source[indexPath.Row]);
            reusableCell.DetailTextLabel.Text = $"{detail}";

            reusableCell.Accessory = _selectedCache.ContainsKey(indexPath.Row) ?
                UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;


            return reusableCell;
        }


        /// <summary>
        /// Numbers the of sections.
        /// </summary>
        /// <returns>The of sections.</returns>
        /// <param name="tableView">Table view.</param>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _source.Count;
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.CellAt(indexPath);

            if (_pickerCell.MaxSelectedNumber == 1) {
                RowSelectedSingle(cell, indexPath.Row);
                DoPickToClose();
            }
            else {
                RowSelectedMulti(cell, indexPath.Row);
            }

            tableView.DeselectRow(indexPath, true);

        }

        void RowSelectedSingle(UITableViewCell cell, int index)
        {
            if (_selectedCache.ContainsKey(index)) {
                return;
            }

            foreach (var vCell in TableView.VisibleCells) {
                vCell.Accessory = UITableViewCellAccessory.None;
            }

            _selectedCache.Clear();
            cell.Accessory = UITableViewCellAccessory.Checkmark;
            _selectedCache[index] = _source[index];
        }

        void RowSelectedMulti(UITableViewCell cell, int index)
        {
            if (_selectedCache.ContainsKey(index)) {
                cell.Accessory = UITableViewCellAccessory.None;
                _selectedCache.Remove(index);
                return;
            }

            if (_pickerCell.MaxSelectedNumber != 0 && _selectedCache.Count() >= _pickerCell.MaxSelectedNumber) {
                return;
            }

            cell.Accessory = UITableViewCellAccessory.Checkmark;
            _selectedCache[index] = _source[index];

            DoPickToClose();
        }

        void DoPickToClose()
        {
            if (_pickerCell.UsePickToClose && _selectedCache.Count == _pickerCell.MaxSelectedNumber)
            {
                this.NavigationController.PopViewController(true);
            }
        }

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Title = _pickerCell.PageTitle;

            var parent = _pickerCell.Parent as SettingsView;
            if (parent != null) {
                TableView.SeparatorColor = parent.SeparatorColor.ToUIColor();
                TableView.BackgroundColor = parent.BackgroundColor.ToUIColor();
            }

            foreach (var item in _pickerCell.SelectedItems) {
                var idx = _source.IndexOf(item);
                if (idx < 0) {
                    continue;
                }
                _selectedCache[idx] = _source[idx];
                if (_pickerCell.MaxSelectedNumber >= 1 && _selectedCache.Count >= _pickerCell.MaxSelectedNumber) {
                    break;
                }
            }

            if (_pickerCell.SelectedItems.Count > 0) {
                var idx = _source.IndexOf(_pickerCell.SelectedItems[0]);
                if(idx < 0){
                    return;
                }

                BeginInvokeOnMainThread(() =>
                {
                    TableView.ScrollToRow(NSIndexPath.Create(new nint[] { 0, idx }), UITableViewScrollPosition.Middle, false);
                });
            }

        }

        /// <summary>
        /// Views the will disappear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillDisappear(bool animated)
        {
            _pickerCell.SelectedItems.Clear();

            foreach (var kv in _selectedCache) {
                _pickerCell.SelectedItems.Add(kv.Value);
            }


            _pickerCellNative.UpdateSelectedItems(true);

            if (_pickerCell.KeepSelectedUntilBack) {
                _tableView.DeselectRow(_tableView.IndexPathForSelectedRow, true);
            }

            _pickerCell.InvokeCommand();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _pickerCell = null;
                _selectedCache = null;
                _source = null;
                _parent = null;
                _accentColor.Dispose();
                _accentColor = null;
                _titleColor?.Dispose();
                _titleColor = null;
                _background?.Dispose();
                _background = null;
                _tableView = null;
            }
            base.Dispose(disposing);
        }


    }
}
