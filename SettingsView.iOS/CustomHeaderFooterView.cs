using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.Reflection;
using System.ComponentModel;
using CoreGraphics;

namespace AiForms.Renderers.iOS
{
    public class CustomHeaderFooterView:UITableViewHeaderFooterView
    {
        // Get internal members
        static BindableProperty RendererProperty = (BindableProperty)typeof(Platform).GetField("RendererProperty", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
        static Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.Platform+DefaultRenderer");
        static Type ModalWrapper = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.ModalWrapper");
        static MethodInfo ModalWapperDispose = ModalWrapper.GetMethod("Dispose");

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


                IVisualElementRenderer renderer;
                if (_rendererRef != null && _rendererRef.TryGetTarget(out renderer) && renderer.Element != null)
                {   
                    DisposeModelAndChildrenRenderers(renderer.Element);
                    _rendererRef = null;
                }


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

            IVisualElementRenderer renderer;
            if (_rendererRef == null || !_rendererRef.TryGetTarget(out renderer))
            {
                renderer = GetNewRenderer();
            }
            else
            {
                if (renderer.Element != null && renderer == Platform.GetRenderer(renderer.Element))
                    renderer.Element.ClearValue(RendererProperty);

                var type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(this._formsCell);
                var reflectableType = renderer as System.Reflection.IReflectableType;
                var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : renderer.GetType();
                if (rendererType == type || (renderer.GetType() == DefaultRenderer) && type == null)
                    renderer.SetElement(this._formsCell);
                else
                {
                    //when cells are getting reused the element could be already set to another cell
                    //so we should dispose based on the renderer and not the renderer.Element
                    DisposeRendererAndChildren(renderer);
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
            ContentView.AddSubview(newRenderer.NativeView);
            return newRenderer;
        }

        // From internal Platform class
        void DisposeModelAndChildrenRenderers(Element view)
        {
            IVisualElementRenderer renderer;
            foreach (VisualElement child in view.Descendants())
            {
                renderer = Platform.GetRenderer(child);
                child.ClearValue(RendererProperty);

                if (renderer != null)
                {
                    renderer.NativeView.RemoveFromSuperview();
                    renderer.Dispose();
                }
            }

            renderer = Platform.GetRenderer((VisualElement)view);
            if (renderer != null)
            {
                if (renderer.ViewController != null)
                {
                    if (renderer.ViewController.ParentViewController.GetType() == ModalWrapper)
                    {
                        var modalWrapper = Convert.ChangeType(renderer.ViewController.ParentViewController, ModalWrapper);
                        ModalWapperDispose.Invoke(modalWrapper, new object[] { });
                    }
                }

                renderer.NativeView.RemoveFromSuperview();
                renderer.Dispose();
            }

            view.ClearValue(RendererProperty);
        }

        // From internal Platform class
        void DisposeRendererAndChildren(IVisualElementRenderer rendererToRemove)
        {
            if (rendererToRemove == null)
                return;

            if (rendererToRemove.Element != null && Platform.GetRenderer(rendererToRemove.Element) == rendererToRemove)
                rendererToRemove.Element.ClearValue(RendererProperty);

            var subviews = rendererToRemove.NativeView.Subviews;
            for (var i = 0; i < subviews.Length; i++)
            {
                var childRenderer = subviews[i] as IVisualElementRenderer;
                if (childRenderer != null)
                    DisposeRendererAndChildren(childRenderer);
            }

            rendererToRemove.NativeView.RemoveFromSuperview();
            rendererToRemove.Dispose();
        }
    }
}
