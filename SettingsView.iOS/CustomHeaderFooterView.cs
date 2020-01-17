using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.Reflection;
using System.ComponentModel;
using CoreGraphics;
using System.Linq;

namespace AiForms.Renderers.iOS
{
    public class CustomHeaderView : CustomHeaderFooterView
    {
        public CustomHeaderView(IntPtr handle) : base(handle)
        {
        }
    }

    public class CustomFooterView : CustomHeaderFooterView
    {
        public CustomFooterView(IntPtr handle) : base(handle)
        {
        }
    }

    public class CustomHeaderFooterView:UITableViewHeaderFooterView
    {
        WeakReference<IVisualElementRenderer> _rendererRef;
        bool _disposed;

        View _formsCell;
        public View FormsCell
        {
            get { return _formsCell; }
            set {
                if (_formsCell == value)
                    return;
                UpdateCell(value);
            }
        }

        public CustomHeaderFooterView(IntPtr handle):base(handle)
        {
        }

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

            var contentFrame = ContentView.Frame;
            var view = FormsCell;

            Layout.LayoutChildIntoBoundingRegion(view, contentFrame.ToRectangle());

            if (_rendererRef == null)
                return;

            IVisualElementRenderer renderer;
            if (_rendererRef.TryGetTarget(out renderer))
                renderer.NativeView.Frame = view.Bounds.ToRectangleF();
                
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
            if(_formsCell != null)
            {
                _formsCell.PropertyChanged -= CellPropertyChanged;
            }
            _formsCell = cell;
            _formsCell.PropertyChanged += CellPropertyChanged;           

            if(ContentView.Subviews.Any())
            {
                ContentView.Subviews[0].RemoveFromSuperview();
            }

            var renderer = CreateOrGetRenderer();
            renderer.NativeView.RemoveFromSuperview();
            ContentView.AddSubview(renderer.NativeView);
           

            UpdateNativeCell();      
        }

        protected virtual IVisualElementRenderer CreateOrGetRenderer()
        {
            var newRenderer = Platform.GetRenderer(_formsCell) ?? Platform.CreateRenderer(_formsCell);
            Platform.SetRenderer(_formsCell, newRenderer);

            if(_rendererRef == null)
            {
                _rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
            }
            else
            {
                _rendererRef.SetTarget(newRenderer);
            }

            return newRenderer;
        }
    }
}
