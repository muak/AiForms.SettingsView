using System;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Xamarin.Forms;
using AiForms.Renderers.iOS.Extensions;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Button cell renderer.
    /// </summary>
    public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

    /// <summary>
    /// Button cell view.
    /// </summary>
    public class ButtonCellView : CellBaseView
    {
        internal Action Execute { get; set; }
        ButtonCell _ButtonCell => Cell as ButtonCell;
        ICommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.ButtonCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public ButtonCellView(Cell formsCell) : base(formsCell)
        {
            DescriptionLabel.Hidden = true;
            SelectionStyle = UIKit.UITableViewCellSelectionStyle.Default;
            TitleLabel.TextAlignment = UIKit.UITextAlignment.Right;
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
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

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateCommand();
            UpdateTitleAlignment();
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

        void UpdateTitleAlignment()
        {
            TitleLabel.TextAlignment = _ButtonCell.TitleAlignment.ToUITextAlignment();
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
                UserInteractionEnabled = true;
                TitleLabel.Alpha = 1f;
                IconView.Alpha = 1f;
            }
            else {
                UserInteractionEnabled = false;
                TitleLabel.Alpha = 0.3f;
                IconView.Alpha = 0.3f;
            }
        }
    }

}
