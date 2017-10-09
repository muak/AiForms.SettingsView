using System;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class NumberPickerCellRenderer:CellBaseRenderer<NumberPickerCellView>{}

    public class NumberPickerCellView : LabelCellView,IPickerCell
    {
        public UITextField DummyField { get; set; }
        NumberPickerSource _model;
        UILabel _titleLabel;
        UIPickerView _picker;
        ICommand _command;

        NumberPickerCell _NumberPikcerCell => Cell as NumberPickerCell;

        public NumberPickerCellView(Cell formsCell):base(formsCell){

            DummyField = new NoCaretField();
            DummyField.BorderStyle = UITextBorderStyle.None;
            DummyField.BackgroundColor = UIColor.Clear;
            ContentView.AddSubview(DummyField);
            ContentView.SendSubviewToBack(DummyField);

            SelectionStyle = UITableViewCellSelectionStyle.Default;

            SetUpPicker();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == NumberPickerCell.MinProperty.PropertyName ||
                e.PropertyName == NumberPickerCell.MaxProperty.PropertyName) {
                UpdateNumberList();
            }
            else if (e.PropertyName == NumberPickerCell.NumberProperty.PropertyName) {
                UpdateNumber();
            }
            else if (e.PropertyName == NumberPickerCell.PickerTitleProperty.PropertyName) {
                UpdateTitle();
            }
            else if(e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName){
                UpdateCommand();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateNumberList();
            UpdateNumber();
            UpdateTitle();
            UpdateCommand();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                DummyField.RemoveFromSuperview();
                DummyField?.Dispose();
                DummyField = null;
                _titleLabel?.Dispose();
                _titleLabel = null;
                _model?.Dispose();
                _model = null;
                _picker?.Dispose();
                _picker = null;
                _command = null;

            }
            base.Dispose(disposing);
        }

        void SetUpPicker()
        {
            _picker = new UIPickerView();

            _titleLabel = new UILabel();

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => {
                DummyField.ResignFirstResponder();
                Select(_model.PreSelectedItem);
            });

            var labelButton = new UIBarButtonItem(_titleLabel);
            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => {
                _model.OnUpdatePickerFormModel();
                DummyField.ResignFirstResponder();
                _command?.Execute(_model.SelectedItem);
            });

            toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);


            DummyField.InputView = _picker;
            DummyField.InputAccessoryView = toolbar;

            _model = new NumberPickerSource();
            _picker.Model = _model;

            _model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
        }

        void UpdateNumber()
        {
            Select(_NumberPikcerCell.Number);
            ValueLabel.Text = _NumberPikcerCell.Number.ToString();
        }

        void UpdateNumberList()
        {
            _model.SetNumbers(_NumberPikcerCell.Min, _NumberPikcerCell.Max);
        }

        void UpdateTitle()
        {
            _titleLabel.Text = _NumberPikcerCell.PickerTitle;
            _titleLabel.SizeToFit();
        }

        void UpdateCommand()
        {
            _command = _NumberPikcerCell.SelectedCommand;
        }

        void Model_UpdatePickerFromModel(object sender, EventArgs e)
        {
            _NumberPikcerCell.Number = _model.SelectedItem;
            ValueLabel.Text = _model.SelectedItem.ToString();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
        }

        void Select(int number)
        {
            var idx = _model.Items.IndexOf(number);
            if (idx == -1) {
                number = _model.Items[0];
                idx = 0;
            }
            _picker.Select(idx,0,false);
            _model.SelectedItem = number;
            _model.SelectedIndex = idx;
            _model.PreSelectedItem = number;
        }
    }
}
