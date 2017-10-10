using System;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]
namespace AiForms.Renderers.iOS
{
    public class SettingsViewRenderer : ViewRenderer<SettingsView, UITableView>
    {
        Page _parentPage;
        KeyboardInsetTracker _insetTracker;
        const float MinRowHeight = 44;
        UITableView _tableview;

        bool _disposed = false;

        protected override void OnElementChanged(ElementChangedEventArgs<SettingsView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null) {
                
                _tableview = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);
                SetNativeControl(_tableview);
                _tableview.ScrollEnabled = true;
                _tableview.RowHeight = UITableView.AutomaticDimension;
                _tableview.Source = new SettingsTableSource(Element);

                _tableview.CellLayoutMarginsFollowReadableWidth = false;

                _tableview.SectionHeaderHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionHeaderHeight = MinRowHeight;

                //Footerの高さを可変にするにはこの2つが必須
                _tableview.SectionFooterHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionFooterHeight = MinRowHeight;

                UpdateBackgroundColor();
                UpdateSeparator();
                UpdateRowHeight();

                Element elm = Element;
                while(elm != null){
                    elm = elm.Parent;
                    if(elm is Page){
                        break;
                    }
                }

                _parentPage = elm as Page;
                _parentPage.Appearing += ParentPageAppearing;

                _insetTracker = new KeyboardInsetTracker(_tableview, () => Control.Window, insets => Control.ContentInset = Control.ScrollIndicatorInsets = insets, point =>
                {
                    var offset = Control.ContentOffset;
                    offset.Y += point.Y;
                    Control.SetContentOffset(offset, true);
                });
            }
        }

        void ParentPageAppearing(object sender, EventArgs e)
        {
            _tableview.DeselectRow(_tableview.IndexPathForSelectedRow, true);
        }


        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return Control.GetSizeRequest(widthConstraint, heightConstraint, MinRowHeight, MinRowHeight);
        }

        protected override void UpdateNativeWidget()
        {
            if (Element.Opacity < 1) {
                if (!Control.Layer.ShouldRasterize) {
                    Control.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
                    Control.Layer.ShouldRasterize = true;
                }
            }
            else
                Control.Layer.ShouldRasterize = false;
            base.UpdateNativeWidget();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == SettingsView.SeparatorColorProperty.PropertyName){
                UpdateSeparator();
            }
            else if(e.PropertyName == SettingsView.BackgroundColorProperty.PropertyName){
                UpdateBackgroundColor();
            }
            else if(e.PropertyName == TableView.RowHeightProperty.PropertyName){
                UpdateRowHeight();
            }
        }


        void UpdateRowHeight()
        {
            _tableview.EstimatedRowHeight = Math.Max((float)Element.RowHeight, MinRowHeight);
            _tableview.ReloadData();        
        }

        void UpdateBackgroundColor()
        {
            var color = Element.BackgroundColor;
            if(color != Color.Default){
                Control.BackgroundColor = color.ToUIColor();
            }
        }

        void UpdateSeparator() 
        {
            var color = Element.SeparatorColor;
            Control.SeparatorColor = color.ToUIColor();
        }

        protected override void Dispose(bool disposing)
        {
            _parentPage.Appearing -= ParentPageAppearing;

            if (_disposed) {
                return;
            }

            if (disposing) {
                _insetTracker?.Dispose();
                _insetTracker = null;
                foreach (UIView subview in Subviews) {
                    DisposeSubviews(subview);
                }

                _tableview = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        void DisposeSubviews(UIView view)
        {
            foreach (UIView subView in view.Subviews) {
                DisposeSubviews(subView);
            }

            view.RemoveFromSuperview();
            view.Dispose();
        }

    }

}
