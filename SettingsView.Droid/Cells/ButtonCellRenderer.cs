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
    /// <summary>
    /// Button cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

    /// <summary>
    /// Button cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ButtonCellView : CellBaseView
    {
        internal Action Execute { get; set; }
        ButtonCell _ButtonCell => Cell as ButtonCell;
        ICommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.ButtonCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public ButtonCellView(Context context, Cell cell) : base(context, cell)
        {
            DescriptionLabel.Visibility = Android.Views.ViewStates.Gone;
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
        /// Rows the selected.
        /// </summary>
        /// <param name="adapter">Adapter.</param>
        /// <param name="position">Position.</param>
        public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
        {
            Execute?.Invoke();
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

        /// <summary>
        /// Updates the is enabled.
        /// </summary>
        protected override void UpdateIsEnabled()
        {
            if (_command != null && !_command.CanExecute(_ButtonCell.CommandParameter)) {
                return;
            }
            base.UpdateIsEnabled();
        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (!CellBase.IsEnabled) {
                return;
            }

            SetEnabledAppearance(_command.CanExecute(_ButtonCell.CommandParameter));
        }
    }

}
