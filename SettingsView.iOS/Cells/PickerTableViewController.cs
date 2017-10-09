using System;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using System.Collections;
using System.Reflection;
using Foundation;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    public class PickerTableViewController:UITableViewController
    {
        
        PickerCell _pickerCell;
        SettingsView _parent;
        IList _source;
        Dictionary<int, object> _selectedCache = new Dictionary<int, object>();
        UIColor _accentColor;

        public PickerTableViewController(PickerCellView pickerCellView)
        {
            _pickerCell = pickerCellView.Cell as PickerCell;
            _parent = pickerCellView.CellParent;
            _source = _pickerCell.ItemsSource as IList;

            if(_pickerCell.SelectedItems == null){
                _pickerCell.SelectedItems = new List<object>();
            }

            if (_pickerCell.AccentColor != Xamarin.Forms.Color.Default){
                _accentColor = _pickerCell.AccentColor.ToUIColor();
            }
            else if (_parent.CellAccentColor != Xamarin.Forms.Color.Default){
                _accentColor = _parent.CellAccentColor.ToUIColor();
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            
            var reusableCell = tableView.DequeueReusableCell("pikcercell");
            if(reusableCell == null){
                reusableCell = new UITableViewCell(UITableViewCellStyle.Default, "pickercell");

                if (_parent != null) {
                    reusableCell.TextLabel.TextColor = _parent.CellTitleColor.ToUIColor();
                    reusableCell.TextLabel.Font = reusableCell.TextLabel.Font.WithSize((System.nfloat)_parent.CellTitleFontSize);
                    reusableCell.BackgroundColor = _parent.CellBackgroundColor.ToUIColor();
                    reusableCell.TintColor = _accentColor;
                }

            }

            var text = _pickerCell.DisplayValue(_source[indexPath.Row]);
            reusableCell.TextLabel.Text = $"{text}";
            reusableCell.Accessory = _selectedCache.ContainsKey(indexPath.Row) ? 
                UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;


            return reusableCell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _source.Count;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.CellAt(indexPath); 

            if(_pickerCell.MaxSelectedNumber == 1){
                RowSelectedSingle(cell,indexPath.Row);
            }
            else{
                RowSelectedMulti(cell,indexPath.Row);
            }

            tableView.DeselectRow(indexPath, true);
        }

        void RowSelectedSingle(UITableViewCell cell,int index)
        {
            if(_selectedCache.ContainsKey(index)){
                return;
            }

            foreach(var vCell in TableView.VisibleCells){
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
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Title = _pickerCell.PageTitle;

            var parent = _pickerCell.Parent as SettingsView;
            if(parent != null){
                TableView.SeparatorColor = parent.SeparatorColor.ToUIColor();
                TableView.BackgroundColor = parent.BackgroundColor.ToUIColor();
            }

            foreach(var item in _pickerCell.SelectedItems){
                var idx = _source.IndexOf(item);
                if(idx < 0){
                    continue;
                }
                RowSelected(TableView,NSIndexPath.Create(new nint[] { 0, idx }));              
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            _pickerCell.SelectedItems.Clear();
            var strList = new List<string>();

            foreach(var kv in _selectedCache){
                _pickerCell.SelectedItems.Add(kv.Value);
                strList.Add(_pickerCell.DisplayValue(kv.Value).ToString());
            }

            //TODO: 後で自動でテキストをセットするか、またセットする場所（Value or Description）にするか選択可能にする
            _pickerCell.ValueText = string.Join(",", strList.ToArray());
            //TODO: それから選択終了で発火するコマンドがあっても良いかも
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _pickerCell = null;
                _selectedCache = null;
                _source = null;
                _parent = null;
            }
            base.Dispose(disposing);
        }


    }
}
