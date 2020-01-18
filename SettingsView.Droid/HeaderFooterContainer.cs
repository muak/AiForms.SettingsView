using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    internal class HeaderFooterContainer : FrameLayout, INativeElementView
    {
        public ViewHolder ViewHolder { get; set; }
        public Element Element => FormsCell;
        public bool IsEmpty => _formsCell == null;

        IVisualElementRenderer _renderer;


        public HeaderFooterContainer(Context context) : base(context)
        {
            Clickable = true;
        }

        Xamarin.Forms.View _formsCell;
        public Xamarin.Forms.View FormsCell
        {
            get { return _formsCell; }
            set
            {
                if (_formsCell == value)
                    return;
                UpdateCell(value);
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false; // pass to parent (ripple effect)
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_formsCell != null)
                {
                    _formsCell.PropertyChanged -= CellPropertyChanged;
                    _formsCell = null;
                }

                ViewHolder = null;

                //_renderer?.View?.RemoveFromParent();
                //_renderer?.Dispose();
                _renderer = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (IsEmpty)
            {
                return;
            }

            double width = Context.FromPixels(r - l);
            double height = Context.FromPixels(b - t);


            Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_renderer.Element, new Rectangle(0, 0, width, height));

            _renderer.UpdateLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            if (_renderer == null)
            {
                SetMeasuredDimension(width, 0);
                return;
            }

            SizeRequest measure = _renderer.Element.Measure(Context.FromPixels(width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
            int height = (int)Context.ToPixels(measure.Request.Height);

            SetMeasuredDimension(width, height);
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Cell.IsEnabledProperty.PropertyName)
            {
                UpdateIsEnabled();
            }
        }

        public virtual void UpdateNativeCell()
        {
            UpdateIsEnabled();
        }

        public void UpdateIsEnabled()
        {
            Enabled = _formsCell.IsEnabled;
        }

        protected virtual IVisualElementRenderer CreateOrGetRenderer(Xamarin.Forms.View cell)
        {
            var renderer = Platform.GetRenderer(cell) ?? Platform.CreateRendererWithContext(cell, Context);
            Platform.SetRenderer(cell, renderer);

            return renderer;
        }

        public void UpdateCell(Xamarin.Forms.View cell)
        {
            if (_formsCell != null)
            {
                _formsCell.PropertyChanged -= CellPropertyChanged;
            }

            cell.PropertyChanged += CellPropertyChanged;

            if(_renderer != null)
            {
                RemoveView(_renderer.View);
            }

            _renderer = CreateOrGetRenderer(cell);
            _formsCell = cell;

            _renderer.View.RemoveFromParent();
            AddView(_renderer.View);

            if (_formsCell is Layout viewAsLayout)
            {
                viewAsLayout.ForceLayout();
            }

            UpdateNativeCell();
            Invalidate();

        }
    }
}
