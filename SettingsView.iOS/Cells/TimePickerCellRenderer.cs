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
    public class TimePickerCellRenderer:CellBaseRenderer<TimePickerCellView>{}

    public class TimePickerCellView:LabelCellView,IPickerCell
    {
        TimePickerCell _TimePickerCell => Cell as TimePickerCell;
        UIDatePicker _picker;
        public UITextField DummyField { get; set; }
        UILabel _titleLabel;
        NSDate _preSelectedDate;

        public TimePickerCellView(Cell formsCell):base(formsCell)
        {
            DummyField = new NoCaretField();
            DummyField.BorderStyle = UITextBorderStyle.None;
            DummyField.BackgroundColor = UIColor.Clear;
            ContentView.AddSubview(DummyField);
            ContentView.SendSubviewToBack(DummyField);

            SelectionStyle = UITableViewCellSelectionStyle.Default;

            SetUpTimePicker();
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdatePickerTitle();
            UpdateTime();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if(e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
               e.PropertyName == TimePickerCell.FormatProperty.PropertyName){
                UpdateTime();
            }
            else if (e.PropertyName == TimePickerCell.PickerTitleProperty.PropertyName){
                UpdatePickerTitle();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
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

        void SetUpTimePicker()
        {
            _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };

            _titleLabel = new UILabel();

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => {
                DummyField.ResignFirstResponder();
                Canceled();
            });

            var labelButton = new UIBarButtonItem(_titleLabel);
            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => {
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
        }
    }
}
