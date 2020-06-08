using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Date picker cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class DatePickerCellRenderer : CellBaseRenderer<DatePickerCellView> { }

    /// <summary>
    /// Date picker cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class DatePickerCellView : LabelCellView
    {
        DatePickerCell _datePickerCell => Cell as DatePickerCell;
        DatePickerDialog _dialog;
        Context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.DatePickerCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public DatePickerCellView(Context context, Cell cell) : base(context, cell)
        {
            _context = context;
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateDate();
            UpdateValueTextAlignment();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == DatePickerCell.DateProperty.PropertyName ||
                e.PropertyName == DatePickerCell.FormatProperty.PropertyName) {
                UpdateDate();
            }
            else if (e.PropertyName == DatePickerCell.MaximumDateProperty.PropertyName) {
                UpdateMaximumDate();
            }
            else if (e.PropertyName == DatePickerCell.MinimumDateProperty.PropertyName) {
                UpdateMinimumDate();
            }
            else if ( e.PropertyName == CellBase.ValueTextAlignmentProperty.PropertyName )
            {
	            UpdateValueTextAlignment();
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="adapter">Adapter.</param>
        /// <param name="position">Position.</param>
        public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
        {
            ShowDialog();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (_dialog != null) {
                    _dialog.CancelEvent -= OnCancelButtonClicked;
                    _dialog?.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        void ShowDialog()
        {
            CreateDatePickerDialog(_datePickerCell.Date.Year, _datePickerCell.Date.Month - 1, _datePickerCell.Date.Day);

            UpdateMinimumDate();
            UpdateMaximumDate();

            if (_datePickerCell.MinimumDate > _datePickerCell.MaximumDate) {
                throw new ArgumentOutOfRangeException(
                    nameof(DatePickerCell.MaximumDate),
                    "MaximumDate must be greater than or equal to MinimumDate."
                );
            }

            _dialog.CancelEvent += OnCancelButtonClicked;

            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {

            _dialog = new DatePickerDialog(_context, (o, e) =>
            {
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

        void UpdateValueTextAlignment()
        {
	        vValueLabel.TextAlignment = GetTextAlignment(CellBase.ValueTextAlignment);
        }
        void UpdateMaximumDate()
        {
            if (_dialog != null) {
                //when not to specify 23:59:59,last day can't be selected. 
                _dialog.DatePicker.MaxDate = (long)_datePickerCell.MaximumDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateMinimumDate()
        {
            if (_dialog != null) {
                _dialog.DatePicker.MinDate = (long)_datePickerCell.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

    }
}
