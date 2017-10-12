using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Android.Text.Format;
using Android.Widget;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class TimePickerCellRenderer:CellBaseRenderer<TimePickerCellView>{}

    public class TimePickerCellView:LabelCellView,IPickerCell
    {
        TimePickerCell _TimePickerCell => Cell as TimePickerCell;
        TimePickerDialog _dialog;
        Context _context;
        string _title;

        public TimePickerCellView(Context context,Cell cell):base(context,cell)
        {
            _context = context;
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateTime();
            UpdatePickerTitle();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
               e.PropertyName == TimePickerCell.FormatProperty.PropertyName)
            {
                UpdateTime();
            }
            else if (e.PropertyName == TimePickerCell.PickerTitleProperty.PropertyName)
            {
                UpdatePickerTitle();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _dialog?.Dispose();
                _dialog = null;
            }
            base.Dispose(disposing);
        }

        public void ShowDialog()
        {
            CreateDialog();
        }

        void CreateDialog()
        {

            if (_dialog == null)
            {
                bool is24HourFormat = DateFormat.Is24HourFormat(_context);
                _dialog = new TimePickerDialog(_context, TimeSelected, _TimePickerCell.Time.Hours, _TimePickerCell.Time.Minutes, is24HourFormat);

                var title = new TextView(_context);

                if (!string.IsNullOrEmpty(_title))
                {
                    title.Gravity = Android.Views.GravityFlags.Center;
                    title.SetPadding(10, 10, 10, 10);
                    title.Text = _title;
                    _dialog.SetCustomTitle(title);
                }

                _dialog.SetCanceledOnTouchOutside(true);

                _dialog.DismissEvent += (ss, ee) => {
                    title.Dispose();
                    _dialog.Dispose();
                    _dialog = null;
                };

                _dialog.Show();
            }

        }

        void UpdateTime()
        {
           vValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
        }

        void UpdatePickerTitle()
        {
            _title = _TimePickerCell.PickerTitle;
        }

        void TimeSelected(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            _TimePickerCell.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            UpdateTime();
        }

    }
}
