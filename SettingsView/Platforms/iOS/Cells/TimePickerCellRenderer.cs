using System;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Time picker cell renderer.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

    /// <summary>
    /// Time picker cell view.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class TimePickerCellView : LabelCellView
    {
        TimePickerCell _TimePickerCell => Cell as TimePickerCell;
        UIDatePicker _picker;
        /// <summary>
        /// Gets or sets the dummy field.
        /// </summary>
        /// <value>The dummy field.</value>
        public UITextField DummyField { get; set; }
        UILabel _titleLabel;
        NSDate _preSelectedDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.TimePickerCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public TimePickerCellView(Cell formsCell) : base(formsCell)
        {
            DummyField = new NoCaretField();
            DummyField.BorderStyle = UITextBorderStyle.None;
            DummyField.BackgroundColor = UIColor.Clear;
            ContentView.AddSubview(DummyField);
            ContentView.SendSubviewToBack(DummyField);

            SelectionStyle = UITableViewCellSelectionStyle.Default;

            SetUpTimePicker();
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell(UITableView tableView)
        {
            base.UpdateCell(tableView);
            UpdatePickerTitle();
            UpdateTime();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
               e.PropertyName == TimePickerCell.FormatProperty.PropertyName) {
                UpdateTime();
            }
            else if (e.PropertyName == TimePickerCell.PickerTitleProperty.PropertyName) {
                UpdatePickerTitle();
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            DummyField.BecomeFirstResponder();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                DummyField.RemoveFromSuperview();
                DummyField?.Dispose();
                DummyField = null;
                _picker.Dispose();
                _picker = null;
                _titleLabel?.Dispose();
                _titleLabel = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Layouts the subviews.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
        }

        void SetUpTimePicker()
        {
            _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 4))
            {
                _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            }

            _titleLabel = new UILabel();
            _titleLabel.TextAlignment = UITextAlignment.Center;

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) =>
            {
                DummyField.ResignFirstResponder();
                Canceled();
            });

            var labelButton = new UIBarButtonItem(_titleLabel);
            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
            {
                DummyField.ResignFirstResponder();
                Done();
            });

            toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);

            DummyField.InputView = _picker;
            DummyField.InputAccessoryView = toolbar;


        }

        void Canceled()
        {
            _picker.Date = _preSelectedDate;
        }

        void Done()
        {
            _TimePickerCell.Time = _picker.Date.ToDateTime() - new DateTime(1, 1, 1);
            ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
            _preSelectedDate = _picker.Date;
        }

        void UpdateTime()
        {
            _picker.Date = new DateTime(1, 1, 1).Add(_TimePickerCell.Time).ToNSDate();
            ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
            _preSelectedDate = _picker.Date;
        }

        void UpdatePickerTitle()
        {
            _titleLabel.Text = _TimePickerCell.PickerTitle;
            _titleLabel.SizeToFit();
            _titleLabel.Frame = new CGRect(0, 0, 160, 44);
        }
    }
}
