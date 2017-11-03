using System;
using System.Windows.Input;
using Android.Content;
using Xamarin.Forms;
using AiForms.Renderers.Droid.Extensions;
using AiForms.Renderers;
using AiForms.Renderers.Droid;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

    public class ButtonCellView : CellBaseView
    {
        public Action Execute { get; set; }
        ButtonCell _ButtonCell => Cell as ButtonCell;
        ICommand _command;

        public ButtonCellView(Context context, Cell cell) : base(context, cell)
        {
            DescriptionLabel.Visibility = Android.Views.ViewStates.Gone;
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
                e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName) {
                UpdateCommand();
            }
            else if (e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName) {
                UpdateTitleAlignment();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateCommand();
            UpdateTitleAlignment();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (_command != null) {
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;
            }
            base.Dispose(disposing);
        }

        void UpdateTitleAlignment()
        {
            TitleLabel.Gravity = _ButtonCell.TitleAlignment.ToGravityFlags();
        }

        void UpdateCommand()
        {
            if (_command != null) {
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = _ButtonCell.Command;

            if (_command != null) {
                _command.CanExecuteChanged += Command_CanExecuteChanged;
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () =>
            {
                if (_command == null) {
                    return;
                }
                if (_command.CanExecute(_ButtonCell.CommandParameter)) {
                    _command.Execute(_ButtonCell.CommandParameter);
                }
            };


        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (_command.CanExecute(_ButtonCell.CommandParameter)) {
                Focusable = false;
                DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
                TitleLabel.Alpha = 1f;
                IconView.Alpha = 1f;
            }
            else {
                // not to invoke a ripple effect and not to selected
                Focusable = true;
                DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;
                // to turn like disabled
                TitleLabel.Alpha = 0.3f;
                IconView.Alpha = 0.3f;
            }
        }
    }

}
