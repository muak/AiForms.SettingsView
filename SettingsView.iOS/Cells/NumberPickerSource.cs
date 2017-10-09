using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace AiForms.Renderers.iOS
{
    public class NumberPickerSource : UIPickerViewModel
    {
        
        public IList<int> Items { get; private set;}

        public event EventHandler UpdatePickerFromModel;


        public NumberPickerSource()
        {

        }

        public int SelectedIndex { get; internal set; }

        public int SelectedItem { get; internal set; }

        public int PreSelectedItem { get; set;}

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return Items != null ? Items.Count : 0;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return Items[(int)row].ToString();
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            
            if (Items.Count == 0) {
                SelectedItem = 0;
                SelectedIndex = -1;
            }
            else {
                SelectedItem = Items[(int)row];
                SelectedIndex = (int)row;
            }

        }

        public void SetNumbers(int min, int max)
        {
            if (min < 0) min = 0;
            if (max < 0) max = 0;
            Items = Enumerable.Range(min, max - min + 1).ToList();
        }

        public void OnUpdatePickerFormModel()
        {
            PreSelectedItem = SelectedItem;
            UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
        }
    }
}
