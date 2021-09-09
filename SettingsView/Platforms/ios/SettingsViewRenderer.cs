using System;
using System.Collections.Specialized;
using System.Linq;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using Foundation;
using MobileCoreServices;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Settings view renderer.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class SettingsViewRenderer : ViewRenderer<SettingsView, UITableView>,IUITableViewDragDelegate,IUITableViewDropDelegate
    {
        internal static readonly string TextHeaderId = "textHeaderView";
        internal static readonly string TextFooterId = "textFooterView";
        internal static readonly string CustomHeaderId = "customHeaderView";
        internal static readonly string CustomFooterId = "customFooterView";
        Page _parentPage;
        KeyboardInsetTracker _insetTracker;
        internal static float MinRowHeight = 48;
        UITableView _tableview;
        IDisposable _contentSizeObserver;

        bool _disposed = false;
        float _topInset = 0f;

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<SettingsView> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                e.OldElement.CollectionChanged -= OnCollectionChanged;
                e.OldElement.SectionCollectionChanged -= OnSectionCollectionChanged;
                e.OldElement.SectionPropertyChanged -= OnSectionPropertyChanged;
                e.OldElement.CellPropertyChanged -= OnCellPropertyChanged;
            }

            if (e.NewElement != null) 
            {
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
                _tableview.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

                _tableview.CellLayoutMarginsFollowReadableWidth = false;

                _tableview.SectionHeaderHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionHeaderHeight = UITableView.AutomaticDimension;

                _tableview.SectionFooterHeight = UITableView.AutomaticDimension;
                _tableview.EstimatedSectionFooterHeight = UITableView.AutomaticDimension;

                _tableview.RegisterClassForHeaderFooterViewReuse(typeof(TextHeaderView), TextHeaderId);
                _tableview.RegisterClassForHeaderFooterViewReuse(typeof(TextFooterView), TextFooterId);
                _tableview.RegisterClassForHeaderFooterViewReuse(typeof(CustomHeaderView), CustomHeaderId);
                _tableview.RegisterClassForHeaderFooterViewReuse(typeof(CustomFooterView), CustomFooterId);

                e.NewElement.CollectionChanged += OnCollectionChanged;
                e.NewElement.SectionCollectionChanged += OnSectionCollectionChanged; 
                e.NewElement.SectionPropertyChanged += OnSectionPropertyChanged;
                e.NewElement.CellPropertyChanged += OnCellPropertyChanged;

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

                _contentSizeObserver = _tableview.AddObserver("contentSize", NSKeyValueObservingOptions.New, OnContentSizeChanged);
                
            }
        }        

        void OnContentSizeChanged(NSObservedChange change)
        {
            Element.VisibleContentHeight = Control.ContentSize.Height;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSections(e);
        }

        void OnSectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var sectionIdx = Element.Model.GetSectionIndex((Section)sender);
            UpdateItems(e, sectionIdx, false);
        }

        void OnSectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == Section.IsVisibleProperty.PropertyName)
            {
                UpdateSectionVisible((Section)sender);
            }
            else if(e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
                    e.PropertyName == Section.HeaderViewProperty.PropertyName ||
                    e.PropertyName == Section.HeaderHeightProperty.PropertyName ||
                    e.PropertyName == Section.FooterTextProperty.PropertyName ||
                    e.PropertyName == Section.FooterViewProperty.PropertyName)
            {
                UpdateSectionNoAnimation((Section)sender);
            }
            else if(e.PropertyName == Section.FooterVisibleProperty.PropertyName)
            {
                UpdateSectionFade((Section)sender);
            }
        }

        void OnCellPropertyChanged(object sender, CellPropertyChangedEventArgs e)
        {
            if(e.PropertyName == CellBase.IsVisibleProperty.PropertyName)
            {
                UpdateCellVisible(e.Section,(CellBase)sender);
            }
        }

        void UpdateSectionVisible(Section section)
        {
            var secIndex = Element.Model.GetSectionIndex(section);
            Control.BeginUpdates();
            Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Automatic);
            Control.EndUpdates();
        }

        void UpdateCellVisible(Section section, CellBase cell)
        {
            var secIndex = Element.Model.GetSectionIndex(section);
            var rowIndex = section.IndexOf(cell);
            Control.BeginUpdates();
            Control.ReloadRows(GetPaths(secIndex,rowIndex,1), UITableViewRowAnimation.Automatic);
            Control.EndUpdates();
        }

        void UpdateSectionNoAnimation(Section section)
        {
            var secIndex = Element.Model.GetSectionIndex(section);
            Control.BeginUpdates();
            Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.None);
            Control.EndUpdates();
        }

        void UpdateSectionFade(Section section)
        {
            var secIndex = Element.Model.GetSectionIndex(section);
            Control.BeginUpdates();
            Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Fade);
            Control.EndUpdates();
        }


        void UpdateSections(NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    Control.BeginUpdates();
                    Control.InsertSections(NSIndexSet.FromIndex(e.NewStartingIndex), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    Control.BeginUpdates();
                    Control.DeleteSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    Control.BeginUpdates();
                    Control.ReloadSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:

                    Control.ReloadData();
                    return;
            }
        }

        void UpdateItems(NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped)
        {
            // This means the UITableView hasn't rendered any cells yet
            // so there's no need to synchronize the rows on the UITableView
            if (Control.IndexPathsForVisibleRows == null && e.Action != NotifyCollectionChangedAction.Reset)
                return;

            // If the section is not visible, do nothing.
            if (!Element.Model.GetSection(section).IsVisible)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }                    

                    Control.BeginUpdates();
                    Control.InsertRows(GetPaths(section, e.NewStartingIndex, e.NewItems.Count), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1 )
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                        
                    Control.BeginUpdates();
                    Control.DeleteRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                        
                    Control.BeginUpdates();
                    for (var i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldi = e.OldStartingIndex;
                        var newi = e.NewStartingIndex;

                        if (e.NewStartingIndex < e.OldStartingIndex)
                        {
                            oldi += i;
                            newi += i;
                        }

                        Control.MoveRow(NSIndexPath.FromRowSection(oldi, section), NSIndexPath.FromRowSection(newi, section));
                    }
                    Control.EndUpdates();
                   
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                        
                    Control.BeginUpdates();
                    Control.ReloadRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.None);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Reset:
                    Control.ReloadData();
                    return;
            }
        }

        protected virtual NSIndexPath[] GetPaths(int section, int index, int count)
        {
            var paths = new NSIndexPath[count];
            for (var i = 0; i < paths.Length; i++)
            {
                paths[i] = NSIndexPath.FromRowSection(index + i, section);
            }

            return paths;
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
                if(_tableview.NumberOfSections() == 0)
                {
                    Element.ScrollToTop = false;
                    return;
                }
                var sectionIdx = 0;
                var rows = _tableview.NumberOfRowsInSection(sectionIdx);
                if (rows > 0)
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
                if(sectionIdx < 0)
                {
                    Element.ScrollToBottom = false;
                    return;
                }

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
                _contentSizeObserver.Dispose();
                _contentSizeObserver = null;
                Element.CollectionChanged -= OnCollectionChanged;
                Element.SectionCollectionChanged -= OnSectionCollectionChanged;
                Element.SectionPropertyChanged -= OnSectionPropertyChanged;
                Element.CellPropertyChanged -= OnCellPropertyChanged;
                _insetTracker?.Dispose();
                _insetTracker = null;
                _tableview.Source = null;
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

            var cell = Element.Model.GetCell(indexPath.Section, indexPath.Row);
            if(!cell.IsEnabled)
            {
                return new UIDragItem[] { };
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


                var section = Element.Model.GetSection(secIdx);
                var destSection = Element.Model.GetSection(destinationIndexPath.Section);
                if(!destSection.UseDragSort)
                {
                    return;
                }

                // save scroll position
                var offset = Control.ContentOffset;
                var fromCell = Control.CellAt(NSIndexPath.FromRowSection(rowIdx, secIdx));                

                if (section.ItemsSource == null)
                {
                    // Don't use PerformBatchUpdates. Because can't cancel animations well.
                    Control.BeginUpdates();

                    var cell = section.DeleteCellWithoutNotify(rowIdx);
                    destSection.InsertCellWithoutNotify(cell, destinationIndexPath.Row);                        
                    Control.DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
                    Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                    Control.EndUpdates();

                    Element.SendItemDropped(destSection, cell);
                }
                else
                {                  
                    // Don't use PerformBatchUpdates. Because can't cancel animations well.
                    Control.BeginUpdates();

                    var deletedSet = section.DeleteSourceItemWithoutNotify(rowIdx);
                    destSection.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, destinationIndexPath.Row);                    
                    Control.DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
                    Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                    Control.EndUpdates();
                    Element.SendItemDropped(destSection, deletedSet.Cell);
                }

                // Cancel animations and restore the scroll position.
                var toCell = Control.CellAt(destinationIndexPath);
                toCell?.Layer?.RemoveAllAnimations();
                fromCell?.Layer?.RemoveAllAnimations();
                Control.Layer.RemoveAllAnimations();
                Control.SetContentOffset(offset, false);

                // nothing occur, even if use the following code.
                //coordinator.DropItemToRow(coordinator.Items.First().DragItem, destinationIndexPath);
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

        [Export("tableView:dropSessionDidEnter:")]
        public void DropSessionDidEnter(UITableView tableView, IUIDropSession session)
        {            
        }

        [Export("tableView:dropSessionDidEnd:")]
        public void DropSessionDidEnd(UITableView tableView, IUIDropSession session)
        {            
        }

        [Export("tableView:dropSessionDidExit:")]
        public void DropSessionDidExit(UITableView tableView, IUIDropSession session)
        {
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
                    return new UITableViewDropProposal(UIDropOperation.Move, UITableViewDropIntent.Automatic);
                }
            }

            return new UITableViewDropProposal(UIDropOperation.Cancel);
        }
    }

}
