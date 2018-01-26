using System;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.Droid
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
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.CommandCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public CommandCellView(Context context, Cell cell) : base(context, cell)
        {
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

        protected override void UpdateIsEnabled()
        {
            if (_command != null && !_command.CanExecute(_CommandCell.CommandParameter)) {
                return;
            }
            base.UpdateIsEnabled();
        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (!CellBase.IsEnabled) {
                return;
            }

            SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
        }
    }
}
