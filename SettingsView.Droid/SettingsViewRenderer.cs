using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Graphics.Drawables;
using System.Linq;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Settings view renderer.
    /// </summary>
    /// 
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SettingsViewRenderer : ViewRenderer<SettingsView, RecyclerView>
    {
        Page _parentPage;
        SettingsViewRecyclerAdapter _adapter;
        LinearLayoutManager _layoutManager;
        ItemTouchHelper _itemTouchhelper;
        SettingsViewSimpleCallback _simpleCallback;
        SVItemdecoration _itemDecoration;
        Drawable _divider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SettingsViewRenderer"/> class.
        /// </summary>
        public SettingsViewRenderer(Context context):base(context)
        {
            AutoPackage = false;
        }

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<SettingsView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null) 
            {
                var recyclerView = new RecyclerView(Context);
                _layoutManager = new LinearLayoutManager(Context);
                recyclerView.SetLayoutManager(_layoutManager);

                _divider = Context.GetDrawable(Resource.Drawable.divider);
                _itemDecoration = new SVItemdecoration(_divider,e.NewElement);
                recyclerView.AddItemDecoration(_itemDecoration);

                SetNativeControl(recyclerView);

                Control.Focusable = false;
                Control.DescendantFocusability = DescendantFocusability.AfterDescendants;

                UpdateSeparatorColor();
                UpdateBackgroundColor();
                UpdateRowHeight();

                _adapter = new SettingsViewRecyclerAdapter(Context,e.NewElement,recyclerView);
                Control.SetAdapter(_adapter);

                _simpleCallback = new SettingsViewSimpleCallback(e.NewElement, ItemTouchHelper.Up | ItemTouchHelper.Down, 0);
                _itemTouchhelper = new ItemTouchHelper(_simpleCallback);
                _itemTouchhelper.AttachToRecyclerView(Control);

                Element elm = Element;
                while (elm != null) {
                    elm = elm.Parent;
                    if (elm is Page) {
                        break;
                    }
                }

                _parentPage = elm as Page;
                _parentPage.Appearing += ParentPageAppearing;

                e.NewElement.Root.CollectionChanged += RootCollectionChanged;
            }
        }

        List<IVisualElementRenderer> _shouldDisposeRenderers = new List<IVisualElementRenderer>();

        void RootCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems == null)
            {
                return;
            }

            foreach(Section section in e.OldItems)
            {
                if(section.HeaderView != null)
                {
                    var header = Platform.GetRenderer(section.HeaderView);
                    if(header != null)
                    {
                        _shouldDisposeRenderers.Add(header);
                    }
                }
                if(section.FooterView != null)
                {
                    var footer = Platform.GetRenderer(section.FooterView);
                    if (footer != null)
                    {
                        _shouldDisposeRenderers.Add(footer);
                    }
                }
            }
        }


        void ParentPageAppearing(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => _adapter.DeselectRow());
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (!changed) return;

            var startPos = _layoutManager.FindFirstCompletelyVisibleItemPosition();
            var endPos = _layoutManager.FindLastCompletelyVisibleItemPosition();

            int totalH = 0;
            for (var i = startPos; i <= endPos; i++) {
                var child = _layoutManager.GetChildAt(i);

                if (child == null) return;

                totalH += _layoutManager.GetChildAt(i).Height;
            }
            Element.VisibleContentHeight = Context.FromPixels(Math.Min(totalH, Control.Height));
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
                UpdateSeparatorColor();
                Control.InvalidateItemDecorations();
            }
            else if (e.PropertyName == SettingsView.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == TableView.RowHeightProperty.PropertyName) {
                UpdateRowHeight();
            }
            else if (e.PropertyName == SettingsView.UseDescriptionAsValueProperty.PropertyName) {
                _adapter.NotifyDataSetChanged();
            }
            else if (e.PropertyName == SettingsView.SelectedColorProperty.PropertyName) {
                //_adapter.NotifyDataSetChanged();
            }
            else if (e.PropertyName == SettingsView.ShowSectionTopBottomBorderProperty.PropertyName) {
                //_adapter.NotifyDataSetChanged();
                Control.InvalidateItemDecorations();
            }
            else if (e.PropertyName == TableView.HasUnevenRowsProperty.PropertyName) {
                _adapter.NotifyDataSetChanged();
            }
            else if (e.PropertyName == SettingsView.ScrollToTopProperty.PropertyName){
                UpdateScrollToTop();
            }
            else if (e.PropertyName == SettingsView.ScrollToBottomProperty.PropertyName){
                UpdateScrollToBottom();
            }
        }

        void UpdateSeparatorColor()
        {
            _divider.SetTint(Element.SeparatorColor.ToAndroid());
        }

        void UpdateRowHeight()
        {
            if (Element.RowHeight == -1) {
                Element.RowHeight = 60;
            }
            else {
                _adapter?.NotifyDataSetChanged();
            }
        }

        void UpdateScrollToTop()
        {
            if (Element.ScrollToTop)
            {
                _layoutManager.ScrollToPosition(0);
                Element.ScrollToTop = false;
            }
        }

        void UpdateScrollToBottom()
        {
            if (Element.ScrollToBottom)
            {              
                if(_adapter != null ){
                    _layoutManager.ScrollToPosition(_adapter.ItemCount - 1);
                }
                Element.ScrollToBottom = false;
            }
        }

        new void UpdateBackgroundColor()
        {
            if (Element.BackgroundColor != Xamarin.Forms.Color.Default) {
                Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
            }
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
                foreach (var section in Element.Root)
                {
                    if (section.HeaderView != null)
                    {
                        DisposeChildRenderer(section.HeaderView);
                    }
                    if (section.FooterView != null)
                    {
                        DisposeChildRenderer(section.FooterView);
                    }
                }

                foreach(var renderer in _shouldDisposeRenderers)
                {
                    if(renderer.View.Handle != IntPtr.Zero)
                    {
                        renderer.View.RemoveFromParent();
                        renderer.View.Dispose();
                    }
                    renderer.Dispose();
                }
                _shouldDisposeRenderers.Clear();
                _shouldDisposeRenderers = null;

                Control.RemoveItemDecoration(_itemDecoration);
                _parentPage.Appearing -= ParentPageAppearing;
                _adapter?.Dispose();
                _adapter = null;
                _layoutManager?.Dispose();
                _layoutManager = null;
                _simpleCallback?.Dispose();
                _simpleCallback = null;
                _itemTouchhelper?.Dispose();
                _itemTouchhelper = null;

                _itemDecoration?.Dispose();
                _itemDecoration = null;
                _divider?.Dispose();
                _divider = null;

                Element.Root.CollectionChanged -= RootCollectionChanged;
            }
            base.Dispose(disposing);
        }

        void DisposeChildRenderer(Xamarin.Forms.View view)
        {
            var renderer = Platform.GetRenderer(view);
            if(renderer != null)
            {
                if(renderer.View.Handle != IntPtr.Zero)
                {
                    renderer.View.RemoveFromParent();
                    renderer.View.Dispose();
                }
                renderer.Dispose();
            }
        }

    }

    [Android.Runtime.Preserve(AllMembers = true)]
    class SettingsViewSimpleCallback : ItemTouchHelper.SimpleCallback
    {
        SettingsView _settingsView;
        RowInfo _fromInfo;
        RowInfo _toInfo;

        public SettingsViewSimpleCallback(SettingsView settingsView,int dragDirs,int swipeDirs):base(dragDirs,swipeDirs)
        {
            _settingsView = settingsView;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            if (!(viewHolder is ContentViewHolder fromContentHolder))
            {
                return false;
            }

            var fromPos = viewHolder.AdapterPosition;
            var toPos = target.AdapterPosition;

            if(fromPos < toPos)
            {
                // disallow a Footer when drag is from up to down.
                if (target is IFooterViewHolder)
                {
                    _toInfo = null;
                    return false;
                }
            }
            else
            {
                // disallow a Header when drag is from down to up.
                if (target is IHeaderViewHolder)
                {
                    _toInfo = null;
                    return false;
                }
            }

            var toContentHolder = target as ViewHolder;

            var section = fromContentHolder.RowInfo.Section;
            if(section == null || !section.UseDragSort){
                return false;
            }

            var toSection = toContentHolder.RowInfo.Section;
            if(toSection == null || !toSection.UseDragSort)
            {
                return false;
            }

            _toInfo = toContentHolder.RowInfo;

            var settingsAdapter = recyclerView.GetAdapter() as SettingsViewRecyclerAdapter;

            settingsAdapter.CellMoved(fromPos, toPos); //caches update
            settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update

            return true;
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            base.ClearView(recyclerView, viewHolder);

            var contentHolder = viewHolder as ContentViewHolder;
            if (contentHolder == null)
            {
                return;
            }
            if(_toInfo == null)
            {
                return;
            }

            var fromSection = _fromInfo.Section;
            var fromPos = fromSection.IndexOf(_fromInfo.Cell);

            var toSection = _toInfo.Section;
            var toPos = toSection.IndexOf(_toInfo.Cell);
            if(fromSection != toSection)
            {
                toPos++;
            }

            if (fromSection.ItemsSource == null) 
            {
                var cell = fromSection.DeleteCellWithoutNotify(fromPos);
                toSection.InsertCellWithoutNotify(cell, toPos);
            }
            else
            {
                // must update DataSource at this timing.
                var deletedSet = fromSection.DeleteSourceItemWithoutNotify(fromPos);
                toSection.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, toPos);
            }

            _fromInfo.Section = _toInfo.Section;

            _toInfo = null;
            _fromInfo = null;
        }

        public override int GetDragDirs(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            var contentHolder = viewHolder as ContentViewHolder;
            if (contentHolder == null)
            {
                return 0;
            }

            var section = contentHolder.RowInfo.Section;
            if (section == null || !section.UseDragSort)
            {
                return 0;
            }

            _fromInfo = contentHolder.RowInfo;
            return base.GetDragDirs(recyclerView, viewHolder);
        }


        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _settingsView = null;
            }
            base.Dispose(disposing);
        }
    }

}
