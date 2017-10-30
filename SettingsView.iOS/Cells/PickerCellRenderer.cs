using System;
using System.Collections.Generic;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.iOS
{                 
    public class PickerCellRenderer:CellBaseRenderer<PickerCellView>{}

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
            if(e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName){
                
            }
            else if(e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
                    e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName){
                UpdateSelectedItems(true);
            }
            else if(e.PropertyName == PickerCell.KeepSelectedUntilBackProperty.PropertyName){
                
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateSelectedItems();
        }

        public void UpdateSelectedItems(bool force = false)
        {
            if (force || string.IsNullOrEmpty(_valueTextCache))
            {
                var strList = new List<string>();
                foreach (var item in _PickerCell.SelectedItems)
                {
                    strList.Add(_PickerCell.DisplayValue(item).ToString());
                }

                _valueTextCache = string.Join(",", strList.OrderBy(x=>x,new NaturalComparer()).ToArray());
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
