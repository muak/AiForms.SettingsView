using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class DatePickerCellRenderer:CellBaseRenderer<DatePickerCellView>{}

    public class DatePickerCellView:LabelCellView,IPickerCell
    {
        DatePickerCell _datePickerCell => Cell as DatePickerCell;
        DatePickerDialog _dialog;
        Context _context;

        public DatePickerCellView(Context context,Cell cell):base(context,cell)
        {
            _context = context;
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateDate();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == DatePickerCell.DateProperty.PropertyName ||
                e.PropertyName == DatePickerCell.FormatProperty.PropertyName)
            {
                UpdateDate();
            }
            else if (e.PropertyName == DatePickerCell.MaximumDateProperty.PropertyName)
            {
                UpdateMaximumDate();
            }
            else if (e.PropertyName == DatePickerCell.MinimumDateProperty.PropertyName)
            {
                UpdateMinimumDate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                if(_dialog != null){
                    _dialog.CancelEvent -= OnCancelButtonClicked;
                    _dialog?.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        public void ShowDialog()
        {
            CreateDatePickerDialog(_datePickerCell.Date.Year, _datePickerCell.Date.Month - 1, _datePickerCell.Date.Day);

            UpdateMinimumDate();
            UpdateMaximumDate();

            _dialog.CancelEvent += OnCancelButtonClicked;

            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {

            _dialog = new DatePickerDialog(_context, (o, e) => {
                _datePickerCell.Date = e.Date;
                ClearFocus();
                _dialog.CancelEvent -= OnCancelButtonClicked;

                _dialog = null;
            }, year, month, day);
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            ClearFocus();
        }

        void UpdateDate()
        {
            var format = _datePickerCell.Format;
            vValueLabel.Text = _datePickerCell.Date.ToString(format);
        }

        void UpdateMaximumDate()
        {
            if (_dialog != null)
            {
                //when not to specify 23:59:59,last day can't be selected. 
                _dialog.DatePicker.MaxDate = (long)_datePickerCell.MaximumDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateMinimumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MinDate = (long)_datePickerCell.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }
      
    }
}
