using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Xamarin.Forms;
using Android.Support.V7.Widget;
using System;
using System.Windows.Input;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class CommandCellRenderer:CellBaseRenderer<CommandCellView>{}

    public class CommandCellView : LabelCellView
    {
        public System.Action Execute { get; set; }
        CommandCell _CommandCell => Cell as CommandCell;
        ICommand _command;

        public CommandCellView(Context context, Cell cell) : base(context, cell)
        {
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CommandCell.CommandProperty.PropertyName ||
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName)
            {
                UpdateCommand();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateCommand();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_command != null) {
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand()
        {
            if(_command != null){
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = _CommandCell.Command;

            if (_command != null) {
                _command.CanExecuteChanged += Command_CanExecuteChanged;              
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () => {
                if (_command == null)
                {
                    return;
                }
                if (_command.CanExecute(_CommandCell.CommandParameter))
                {
                    _command.Execute(_CommandCell.CommandParameter);
                }
            };


        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (_command.CanExecute(_CommandCell.CommandParameter)) {
                Focusable = false;
                DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
                TitleLabel.Alpha = 1f;
                DescriptionLabel.Alpha = 1f;
                ValueLabel.Alpha = 1f;
            }
            else {
                // not to invoke a ripple effect and not to selected
                Focusable = true;
                DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;
                // to turn like disabled
                TitleLabel.Alpha = 0.6f;
                DescriptionLabel.Alpha = 0.6f;
                ValueLabel.Alpha = 0.6f;
            }
        }
    }
}
