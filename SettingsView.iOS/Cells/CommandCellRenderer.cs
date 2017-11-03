using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Xamarin.Forms;
using System;
using UIKit;
using System.Windows.Input;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Command cell renderer.
    /// </summary>
    public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

    /// <summary>
    /// Command cell view.
    /// </summary>
    public class CommandCellView : LabelCellView
    {
        internal Action Execute { get; set; }
        CommandCell _CommandCell => Cell as CommandCell;
        ICommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CommandCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public CommandCellView(Cell formsCell) : base(formsCell)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            SelectionStyle = UITableViewCellSelectionStyle.Default;
            SetRightMarginZero();

        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CommandCell.CommandProperty.PropertyName ||
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName) {
                UpdateCommand();
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateCommand();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
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

        void UpdateCommand()
        {
            if (_command != null) {
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = _CommandCell.Command;

            if (_command != null) {
                _command.CanExecuteChanged += Command_CanExecuteChanged;
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () =>
            {
                if (_command == null) {
                    return;
                }
                if (_command.CanExecute(_CommandCell.CommandParameter)) {
                    _command.Execute(_CommandCell.CommandParameter);
                }
            };

        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (_command.CanExecute(_CommandCell.CommandParameter)) {
                UserInteractionEnabled = true;
                TitleLabel.Alpha = 1f;
                DescriptionLabel.Alpha = 1f;
                ValueLabel.Alpha = 1f;
            }
            else {
                UserInteractionEnabled = false;
                TitleLabel.Alpha = 0.6f;
                DescriptionLabel.Alpha = 0.6f;
                ValueLabel.Alpha = 0.6f;
            }
        }
    }

}
