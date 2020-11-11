using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class CustomCellContent:UIView
    {
        WeakReference<IVisualElementRenderer> _rendererRef;
        bool _disposed;
        NSLayoutConstraint _heightConstraint;
        View _formsCell;
        public CustomCell CustomCell { get; set; }
        double _lastFrameWidth = -9999d;
        double _lastMeasureWidth = -9999d;
        double _lastMeasureHeight = -9999d;

        public CustomCellContent() { }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_formsCell != null)
                {
                    _formsCell.PropertyChanged -= CellPropertyChanged;
                }

                CustomCell = null;

                _heightConstraint?.Dispose();
                _heightConstraint = null;

                IVisualElementRenderer renderer = null;
                if (_rendererRef != null && _rendererRef.TryGetTarget(out renderer) && renderer.Element != null)
                {
                    FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
                    _rendererRef = null;
                }

                renderer?.Dispose();

                _formsCell = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        public virtual void UpdateNativeCell()
        {
            UpdateIsEnabled();
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Cell.IsEnabledProperty.PropertyName)
            {
                UpdateIsEnabled();
            }
        }

        protected virtual void UpdateIsEnabled()
        {
            UserInteractionEnabled = _formsCell.IsEnabled;
        }        

        public virtual void UpdateCell(View cell,UITableView tableView)
        {
            if (_formsCell == cell && !CustomCell.IsForceLayout)
            {
                return;
            }
            CustomCell.IsForceLayout = false;

            if (_formsCell != null)
            {
                _formsCell.PropertyChanged -= CellPropertyChanged;
            }
            _formsCell = cell;
            _formsCell.PropertyChanged += CellPropertyChanged;

            IVisualElementRenderer renderer;
            if (_rendererRef == null || !_rendererRef.TryGetTarget(out renderer))
            {
                renderer = GetNewRenderer();
            }
            else
            {
                if (renderer.Element != null && renderer == Platform.GetRenderer(renderer.Element))
                    renderer.Element.ClearValue(FormsInternals.RendererProperty);

                var type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(this._formsCell);
                var reflectableType = renderer as System.Reflection.IReflectableType;
                var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : renderer.GetType();
                if (rendererType == type || (renderer.GetType() == FormsInternals.DefaultRenderer) && type == null)
                    renderer.SetElement(this._formsCell);
                else
                {
                    //when cells are getting reused the element could be already set to another cell
                    //so we should dispose based on the renderer and not the renderer.Element
                    FormsInternals.DisposeRendererAndChildren(renderer);
                    renderer = GetNewRenderer();
                }
            }

            Platform.SetRenderer(this._formsCell, renderer);

            if (!CustomCell.IsMeasureOnce || tableView.Frame.Width != _lastFrameWidth)
            {
                _lastFrameWidth = tableView.Frame.Width;
                var height = double.PositiveInfinity;               
                var width = tableView.Frame.Width - (CustomCell.UseFullSize ? 0 : 32); // CellBaseView layout margin
                var result = renderer.Element.Measure(tableView.Frame.Width, height, MeasureFlags.IncludeMargins);
                _lastMeasureWidth = result.Request.Width;
                if (_formsCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
                {                    
                    _lastMeasureWidth = width;
                }
                _lastMeasureHeight = result.Request.Height;

                if (_heightConstraint != null)
                {
                    _heightConstraint.Active = false;
                    _heightConstraint?.Dispose();
                }

                _heightConstraint = renderer.NativeView.HeightAnchor.ConstraintEqualTo((nfloat)_lastMeasureHeight);
                _heightConstraint.Priority = 999f;
                _heightConstraint.Active = true;

                renderer.NativeView.UpdateConstraintsIfNeeded();
            }            

            Layout.LayoutChildIntoBoundingRegion(_formsCell, new Rectangle(0, 0, _lastMeasureWidth,_lastMeasureHeight));                       

            UpdateNativeCell();
        }


        protected virtual IVisualElementRenderer GetNewRenderer()
        {
            var newRenderer = Platform.CreateRenderer(_formsCell);
            _rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
            AddSubview(newRenderer.NativeView);

            var native = newRenderer.NativeView;
            native.TranslatesAutoresizingMaskIntoConstraints = false;

            native.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            native.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            native.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            native.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;

            return newRenderer;
        }
    }
}
