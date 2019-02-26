using System;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using ObjCRuntime;
using System.Linq;
using MobileCoreServices;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Settings view renderer.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class SettingsViewRenderer : ViewRenderer<SettingsView, UITableView>,IUITableViewDragDelegate,IUITableViewDropDelegate
    {
        Page _parentPage;
        KeyboardInsetTracker _insetTracker;
        internal static float MinRowHeight = 48;
        UITableView _tableview;

        bool _disposed = false;
        float _topInset = 0f;


        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<SettingsView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null) {

                _tableview = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    _tableview.DragDelegate = this;
                    _tableview.DropDelegate = this;

                    _tableview.DragInteractionEnabled = true;
                    _tableview.Source = new SettingsTableSource(Element);
                }
                else{
                    _tableview.Editing = true;
                    _tableview.AllowsSelectionDuringEditing = true;
                    // When Editing is true, for some reason, UITableView top margin is displayed.
                    // force removing the margin by the following code.
                    _topInset = 36;
                    _tableview.ContentInset = new UIEdgeInsets(-_topInset, 0, 0, 0);

                    _tableview.Source = new SettingsLagacyTableSource(Element);
                }
                

                SetNativeControl(_tableview);
                _tableview.ScrollEnabled = true;
                _tableview.RowHeight = UITableView.AutomaticDimension;


                _tableview.CellLayoutMarginsFollowReadableWidth = false;

                _tableview.SectionHeaderHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionHeaderHeight = MinRowHeight;

                //need the following two because of make footer height variable.
                _tableview.SectionFooterHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionFooterHeight = MinRowHeight;

                UpdateBackgroundColor();
                UpdateSeparator();
                UpdateRowHeight();

                Element elm = Element;
                while (elm != null) {
                    elm = elm.Parent;
                    if (elm is Page) {
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

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return Control.GetSizeRequest(widthConstraint, heightConstraint, MinRowHeight, MinRowHeight);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Element.VisibleContentHeight = Math.Min(Control.ContentSize.Height, Control.Frame.Height);
        }

        /// <summary>
        /// Updates the native widget.
        /// </summary>
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

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.SeparatorColorProperty.PropertyName) {
                UpdateSeparator();
            }
            else if (e.PropertyName == SettingsView.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == TableView.RowHeightProperty.PropertyName) {
                UpdateRowHeight();
            }
            else if (e.PropertyName == SettingsView.ScrollToTopProperty.PropertyName){
                UpdateScrollToTop();
            }
            else if(e.PropertyName == SettingsView.ScrollToBottomProperty.PropertyName){
               UpdateScrollToBottom();
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
            if (color != Color.Default) {
                Control.BackgroundColor = color.ToUIColor();
            }
        }

        void UpdateSeparator()
        {
            var color = Element.SeparatorColor;
            Control.SeparatorColor = color.ToUIColor();
        }

        void UpdateScrollToTop()
        {
            if (Element.ScrollToTop)
            {
                var sectionIdx = 0;
                var rows = _tableview.NumberOfRowsInSection(sectionIdx);

                if (_tableview.NumberOfSections() > 0 && rows > 0)
                {
                    _tableview.SetContentOffset(new CGPoint(0,_topInset), false);
                    //_tableview.ScrollToRow(NSIndexPath.Create(0, 0), UITableViewScrollPosition.Top, false);
                }

                Element.ScrollToTop = false;
            }
        }

        void UpdateScrollToBottom()
        {
            if (Element.ScrollToBottom)
            {   
                var sectionIdx = _tableview.NumberOfSections() - 1;
                var rowIdx = _tableview.NumberOfRowsInSection(sectionIdx) -1;

                if(sectionIdx >= 0 && rowIdx >= 0){
                    _tableview.ScrollToRow(NSIndexPath.Create(sectionIdx,rowIdx),UITableViewScrollPosition.Top,false);
                }

                Element.ScrollToBottom = false;
            }
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            _parentPage.Appearing -= ParentPageAppearing;

            if (_disposed) 
            {
                return;
            }

            if (disposing)
            {
                _insetTracker?.Dispose();
                _insetTracker = null;
                foreach (UIView subview in Subviews) 
                {
                    DisposeSubviews(subview);
                }

                _tableview = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        void DisposeSubviews(UIView view)
        {
            var ver = view as IVisualElementRenderer;

            if (ver == null) 
            {
                foreach (UIView subView in view.Subviews)
                {
                    DisposeSubviews(subView);
                }

                view.RemoveFromSuperview();
            }
                
            view.Dispose();
        }

        /// <summary>
        /// Gets the items for beginning drag session.
        /// </summary>
        /// <returns>The items for beginning drag session.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="session">Session.</param>
        /// <param name="indexPath">Index path.</param>
        public UIDragItem[] GetItemsForBeginningDragSession(UITableView tableView, IUIDragSession session, NSIndexPath indexPath)
        {
            var section = Element.Model.GetSection(indexPath.Section);
            if(!section.UseDragSort){
                return new UIDragItem[]{};
            }

            // set "sectionIndex,rowIndex" as string
            var data = NSData.FromString($"{indexPath.Section},{indexPath.Row}");

            var itemProvider = new NSItemProvider();
            itemProvider.RegisterDataRepresentation(UTType.PlainText, NSItemProviderRepresentationVisibility.All, (completionHandler) =>
            {
                completionHandler(data, null);
                return null;
            });

            return new UIDragItem[] { new UIDragItem(itemProvider) };
        }

        /// <summary>
        /// Performs the drop.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="coordinator">Coordinator.</param>
        public void PerformDrop(UITableView tableView, IUITableViewDropCoordinator coordinator)
        {
            var destinationIndexPath = coordinator.DestinationIndexPath;
            if(destinationIndexPath == null){
                return;
            }

            coordinator.Session.LoadObjects<NSString>(items=>{
                var path = items[0].ToString().Split(new char[]{','}, StringSplitOptions.None).Select(x => int.Parse(x)).ToList();
                var secIdx = path[0];
                var rowIdx = path[1];

                if(secIdx != destinationIndexPath.Section){
                    return;
                }

                var section = Element.Model.GetSection(secIdx);
                if(section.ItemsSource == null){
                    var tmp = section[rowIdx];
                    section.RemoveAt(rowIdx);
                    section.Insert(destinationIndexPath.Row, tmp);
                }
                else{
                    var tmp = section.ItemsSource[rowIdx];
                    section.ItemsSource.RemoveAt(rowIdx);
                    section.ItemsSource.Insert(destinationIndexPath.Row, tmp);
                }
            });

        }

        /// <summary>
        /// Ensure that the drop session contains a drag item with a data representation that the view can consume.
        /// </summary>
        [Export("tableView:canHandleDropSession:")]
        public bool CanHandleDropSession(UITableView tableView, IUIDropSession session)
        {
            return session.CanLoadObjects(typeof(NSString));
        }

        /// <summary>
        /// A drop proposal from a table view includes two items: a drop operation,
        /// typically .move or .copy; and an intent, which declares the action the
        /// table view will take upon receiving the items. (A drop proposal from a
        /// custom view does includes only a drop operation, not an intent.)
        /// </summary>
        [Export("tableView:dropSessionDidUpdate:withDestinationIndexPath:")]
        public UITableViewDropProposal DropSessionDidUpdate(UITableView tableView, IUIDropSession session, NSIndexPath destinationIndexPath)
        {
            if(destinationIndexPath == null){
                return new UITableViewDropProposal(UIDropOperation.Cancel);
            }

            // this dragging is from UITableView.
            if (tableView.HasActiveDrag)
            {
                if (session.Items.Length > 1)
                {
                    return new UITableViewDropProposal(UIDropOperation.Cancel);
                }
                else
                {
                    return new UITableViewDropProposal(UIDropOperation.Move, UITableViewDropIntent.InsertAtDestinationIndexPath);
                }
            }

            return new UITableViewDropProposal(UIDropOperation.Cancel);
        }
    }

}
