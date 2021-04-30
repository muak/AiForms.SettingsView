using System;
using System.Collections.Generic;
using System.Linq;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Resource = global::SettingsView.Resource;

[assembly: ExportRenderer(typeof(AiForms.Renderers.SettingsView), typeof(SettingsViewRenderer))]
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
        SettingsViewLayoutManager _layoutManager;
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
                // Fix scrollbar visibility and flash. https://github.com/xamarin/Xamarin.Forms/pull/10893
                var recyclerView = new RecyclerView(new ContextThemeWrapper(Context, Resource.Style.settingsViewTheme),null,Resource.Attribute.settingsViewStyle);                         
                recyclerView.NestedScrollingEnabled = false;

                // When replaced, No animation.
                //(recyclerView.GetItemAnimator() as DefaultItemAnimator).SupportsChangeAnimations = false;

                _layoutManager = new SettingsViewLayoutManager(Context,e.NewElement);
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
            Device.BeginInvokeOnMainThread(() => _adapter?.DeselectRow());
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
        Queue<(RowInfo from, RowInfo to)> _moveHistory = new Queue<(RowInfo from, RowInfo to)>();

        public SettingsViewSimpleCallback(SettingsView settingsView,int dragDirs,int swipeDirs):base(dragDirs,swipeDirs)
        {
            _settingsView = settingsView;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            System.Diagnostics.Debug.WriteLine("OnMove");
            if (!(viewHolder is ContentBodyViewHolder fromContentHolder))
            {
                System.Diagnostics.Debug.WriteLine("Cannot move no ContentHolder");
                return false; 
            }

            var fromPos = viewHolder.AdapterPosition;
            var toPos = target.AdapterPosition;

            if(fromPos < toPos)
            {
                // disallow a Footer when drag is from up to down.
                if (target is IFooterViewHolder)
                {
                    System.Diagnostics.Debug.WriteLine("Up To Down disallow Footer");
                    return false;
                }
            }
            else
            {
                // disallow a Header when drag is from down to up.
                if (target is IHeaderViewHolder)
                {
                    System.Diagnostics.Debug.WriteLine("Down To Up disallow Header");
                    return false;
                }
            }

            var toContentHolder = target as ViewHolder;

            var section = fromContentHolder.RowInfo.Section;
            if(section == null || !section.UseDragSort){
                System.Diagnostics.Debug.WriteLine("From Section Not UseDragSort");
                return false;
            }

            var toSection = toContentHolder.RowInfo.Section;
            if(toSection == null || !toSection.UseDragSort)
            {
                System.Diagnostics.Debug.WriteLine("To Section Not UseDragSort");
                return false;
            }

            var toInfo = toContentHolder.RowInfo;
            System.Diagnostics.Debug.WriteLine($"Set ToInfo Section:{_settingsView.Root.IndexOf(toInfo.Section)} Cell:{toInfo.Section.IndexOf(toInfo.Cell)}");

            var settingsAdapter = recyclerView.GetAdapter() as SettingsViewRecyclerAdapter;
                        
            settingsAdapter.CellMoved(fromPos, toPos); //caches update
            settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update

            // save moved changes 
            _moveHistory.Enqueue((_fromInfo, toInfo));

            System.Diagnostics.Debug.WriteLine($"Move Completed from:{fromPos} to:{toPos}");

            return true;
        }        

        void DataSourceMoved()
        {
            if (!_moveHistory.Any())
            {
                return;
            }

            var cell = _moveHistory.Peek().from.Cell;
            var section = _moveHistory.Last().to.Section;
            while(_moveHistory.Any())
            {
                var pos = _moveHistory.Dequeue();
                DataSourceMoved(pos.from, pos.to);
            }

            _settingsView.SendItemDropped(section, cell);
        }

        void DataSourceMoved(RowInfo from,RowInfo to)
        {
            var fromPos = from.Section.IndexOf(from.Cell);
            var toPos = to.Section.IndexOf(to.Cell);
            if(toPos < 0)
            {
                // if Header, insert the first.s
                toPos = 0;
            }

            if (from.Section.ItemsSource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Update Sections from:{fromPos} to:{toPos}");
                var cell = from.Section.DeleteCellWithoutNotify(fromPos);
                to.Section.InsertCellWithoutNotify(cell, toPos);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"UpdateSource from:{fromPos} to:{toPos}");
                var deletedSet = from.Section.DeleteSourceItemWithoutNotify(fromPos);
                to.Section.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, toPos);
            }

            from.Section = to.Section;
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            System.Diagnostics.Debug.WriteLine("On ClearView");
            base.ClearView(recyclerView, viewHolder);

            viewHolder.ItemView.Alpha = 1.0f;
            viewHolder.ItemView.ScaleX = 1.0f;
            viewHolder.ItemView.ScaleY = 1.0f;

            // DataSource Update
            DataSourceMoved();

            _moveHistory.Clear();
            _fromInfo = null;
        }

        public override int GetDragDirs(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            var contentHolder = viewHolder as ContentBodyViewHolder;
            if (contentHolder == null)
            {
                return 0;
            }

            var section = contentHolder.RowInfo.Section;
            if (section == null || !section.UseDragSort)
            {
                return 0;
            }

            if(!contentHolder.RowInfo.Cell.IsEnabled)
            {
                return 0;
            }

            // save start info.
            _fromInfo = contentHolder.RowInfo;
            System.Diagnostics.Debug.WriteLine($"DragDirs Section:{_settingsView.Root.IndexOf(_fromInfo.Section)} Cell:{_fromInfo.Section.IndexOf(_fromInfo.Cell)}");
            return base.GetDragDirs(recyclerView, viewHolder);
        }

        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            base.OnSelectedChanged(viewHolder, actionState);
            if(viewHolder == null)
            {
                return;
            }

            if(actionState == ItemTouchHelper.ActionStateDrag)
            {
                viewHolder.ItemView.Alpha = 0.9f;
                viewHolder.ItemView.ScaleX = 1.04f;
                viewHolder.ItemView.ScaleY = 1.04f;
            }            
        }


        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _settingsView = null;
                _moveHistory.Clear();
                _moveHistory = null;
            }
            base.Dispose(disposing);
        }
    }

}
