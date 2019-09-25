using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    public class CustomCellContent:UIView
    {
        WeakReference<IVisualElementRenderer> _rendererRef;
        bool _disposed;

        View _formsCell;
        public View FormsCell {
            get { return _formsCell; }
            set {
                if (_formsCell == value)
                    return;
                UpdateCell(value);
            }
        }

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
            UserInteractionEnabled = FormsCell.IsEnabled;
        }

        public override void LayoutSubviews()
        {
            if (FormsCell == null)
            {
                return;
            }

            //This sets the content views frame.
            base.LayoutSubviews();

            SizeToFit();

            var contentFrame = Frame;
            var view = FormsCell;



            Layout.LayoutChildIntoBoundingRegion(view, contentFrame.ToRectangle());

            if (_rendererRef == null)
                return;

            IVisualElementRenderer renderer;
            if (_rendererRef.TryGetTarget(out renderer))
                renderer.NativeView.Frame = view.Bounds.ToRectangleF();

            var constraint = this.HeightAnchor.ConstraintEqualTo(Frame.Height);
            constraint.Priority = 999f;
            constraint.Active = true;

            UpdateConstraintsIfNeeded();
        }


        public override CGSize SizeThatFits(CGSize size)
        {
            IVisualElementRenderer renderer;
            if (!_rendererRef.TryGetTarget(out renderer))
                return base.SizeThatFits(size);

            if (renderer.Element == null)
                return CGSize.Empty;

            double width = size.Width;
            var height = size.Height > 0 ? size.Height : double.PositiveInfinity;
            var result = renderer.Element.Measure(width, height, MeasureFlags.IncludeMargins);

            return new CGSize(size.Width, (float)result.Request.Height);
        }

        protected virtual void UpdateCell(View cell)
        {
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
            UpdateNativeCell();
        }

        protected virtual IVisualElementRenderer GetNewRenderer()
        {
            var newRenderer = Platform.CreateRenderer(_formsCell);
            _rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
            var asdf = this.Frame;
            AddSubview(newRenderer.NativeView);

            return newRenderer;
        }
    }
}
