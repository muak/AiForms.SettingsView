using System;
using UIKit;
using Xamarin.Forms;
using System.Windows.Input;
using Foundation;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using AiForms.Renderers;
using AiForms.Renderers.iOS;

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
        NSLayoutConstraint _heightConstraint;

        protected WeakReference<IVisualElementRenderer> _rendererRef;
        bool _disposed;

        public CustomCellView(Cell formsCell) : base(formsCell)
        {
            if (CustomCell.ShowArrowIndicator)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
                EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

                SetRightMarginZero();
            }

            SelectionStyle = CustomCell.IsSelectable ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();
            //LayoutIfNeeded();
            //SetNeedsLayout();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            //LayoutIfNeeded();
            //SetNeedsLayout();

            //if(_rendererRef == null)
            //{
            //    return;
            //}
            //if(CustomCell.IsMeasureOnce && CustomCell.Content.Bounds.Width > 0 && CustomCell.Content.Bounds.Height > 0)
            //{
            //    return;
            //}
            //System.Diagnostics.Debug.WriteLine($"Icon: {IconView.Frame.Width}");
            //System.Diagnostics.Debug.WriteLine($"ContentView: {ContentView.Frame.Width}");
            //System.Diagnostics.Debug.WriteLine($"StackV: {StackV.Frame.Width}");
            //System.Diagnostics.Debug.WriteLine($"StackH: {StackH.Frame.Width}");
            //var width = StackV.Frame.Width;
            //var sizeRequest = CustomCell.Content.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            //var height = sizeRequest.Request.Height;

            //Layout.LayoutChildIntoBoundingRegion(CustomCell.Content, new Rectangle(0, 0, width, height));
            //IVisualElementRenderer renderer;
            //if (_rendererRef.TryGetTarget(out renderer))
            //    renderer.NativeView.Frame = CustomCell.Content.Bounds.ToRectangleF();


            //if (_heightConstraint != null)
            //{
            //    _heightConstraint.Active = false;
            //    _heightConstraint?.Dispose();
            //}

            //_heightConstraint = _coreView.HeightAnchor.ConstraintEqualTo((float)height);
            //_heightConstraint.Priority = 999f;
            //_heightConstraint.Active = true;
        }

        protected override void SetUpContentView()
        {
            base.SetUpContentView();

            StackV.RemoveArrangedSubview(ContentStack);
            StackV.RemoveArrangedSubview(DescriptionLabel);
            ContentStack.RemoveFromSuperview();
            DescriptionLabel.RemoveFromSuperview();

            _coreView = new CustomCellContent();

            StackV.AddArrangedSubview(_coreView);
        }

        protected virtual void UpdateContent()
        {
            SetNeedsLayout();
            _coreView.FormsCell = CustomCell.Content;
            SetNeedsLayout();
            //IVisualElementRenderer renderer;
            //if (_rendererRef == null || !_rendererRef.TryGetTarget(out renderer))
            //{
            //    renderer = GetNewRenderer();
            //}
            //else
            //{
            //    if (renderer.Element != null && renderer == Platform.GetRenderer(renderer.Element))
            //        renderer.Element.ClearValue(FormsInternals.RendererProperty);

            //    var type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(CustomCell.Content);
            //    var reflectableType = renderer as System.Reflection.IReflectableType;
            //    var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : renderer.GetType();
            //    if (rendererType == type || (renderer.GetType() == FormsInternals.DefaultRenderer) && type == null)
            //        renderer.SetElement(CustomCell.Content);
            //    else
            //    {
            //        //when cells are getting reused the element could be already set to another cell
            //        //so we should dispose based on the renderer and not the renderer.Element
            //        FormsInternals.DisposeRendererAndChildren(renderer);
            //        renderer = GetNewRenderer();
            //    }
            //}

            //Platform.SetRenderer(CustomCell.Content, renderer);
        }

        protected virtual IVisualElementRenderer GetNewRenderer()
        {
            var newRenderer = Platform.CreateRenderer(CustomCell.Content);
            _rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
            _coreView.AddSubview(newRenderer.NativeView);
            return newRenderer;
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
            //UpdateShowArrowIndicator();
            //UpdateIsSelectable();
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
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;

                IVisualElementRenderer renderer = null;
                if (_rendererRef != null && _rendererRef.TryGetTarget(out renderer) && renderer.Element != null)
                {
                    FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
                    _rendererRef = null;
                }

                renderer?.Dispose();

                _heightConstraint?.Dispose();
                _heightConstraint = null;

                _coreView?.RemoveFromSuperview();
                _coreView?.Dispose();
                _coreView = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected virtual void UpdateShowArrowIndicator()
        {
            if(CustomCell.ShowArrowIndicator)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
                EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
                SetRightMarginZero();
            }
            else
            {
                Accessory = UITableViewCellAccessory.None;
                EditingAccessory = UITableViewCellAccessory.None;
                SetRightMargin();
            }
        }

        protected virtual void UpdateIsSelectable()
        {
            SelectionStyle = CustomCell.IsSelectable ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
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
