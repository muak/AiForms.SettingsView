using System;
using UIKit;
using Xamarin.Forms;
using System.Windows.Input;
using Foundation;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]
namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }

    [Foundation.Preserve(AllMembers = true)]
    public class CustomCellView : CellBaseView
    {
        protected CustomCell CustomCell => Cell as CustomCell;
        protected Action Execute { get; set; }
        protected ICommand _command;
        protected CustomCellContent _coreView;

        public CustomCellView(Cell formsCell) : base(formsCell)
        {
            SelectionStyle = CustomCell.IsSelectable ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();
            LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
        }

        protected override void SetUpContentView()
        {
            base.SetUpContentView();

            if (CustomCell.ShowArrowIndicator)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
                EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

                SetRightMarginZero();
            }

            StackV.RemoveArrangedSubview(ContentStack);
            StackV.RemoveArrangedSubview(DescriptionLabel);
            ContentStack.RemoveFromSuperview();
            DescriptionLabel.RemoveFromSuperview();

            _coreView = new CustomCellContent();

            if (CustomCell.UseFullSize)
            {
                StackH.RemoveArrangedSubview(IconView);
                IconView.RemoveFromSuperview();
                
                StackH.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
                StackH.Spacing = 0;
            }

            StackV.AddArrangedSubview(_coreView);
        }

        protected virtual void UpdateContent()
        {
            _coreView.CustomCell = CustomCell;
            _coreView.FormsCell = CustomCell.Content;           
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
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName)
            {
                UpdateCommand();
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Execute?.Invoke();
            if (!CustomCell.KeepSelectedUntilBack)
            {
                tableView.DeselectRow(indexPath, true);
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateContent();
            UpdateCommand();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;

                _coreView?.RemoveFromSuperview();
                _coreView?.Dispose();
                _coreView = null;
            }

            base.Dispose(disposing);
        }

        protected virtual void UpdateCommand()
        {
            if (_command != null)
            {
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = CustomCell.Command;

            if (_command != null)
            {
                _command.CanExecuteChanged += Command_CanExecuteChanged;
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () => {
                if (_command == null)
                {
                    return;
                }
                if (_command.CanExecute(CustomCell.CommandParameter))
                {
                    _command.Execute(CustomCell.CommandParameter);
                }
            };

        }

        /// <summary>
        /// Updates the is enabled.
        /// </summary>
        protected override void UpdateIsEnabled()
        {
            if (_command != null && !_command.CanExecute(CustomCell.CommandParameter))
            {
                return;
            }
            base.UpdateIsEnabled();
        }

        protected virtual void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (!CellBase.IsEnabled)
            {
                return;
            }

            SetEnabledAppearance(_command.CanExecute(CustomCell.CommandParameter));
        }
    }
}
