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

            }
            base.Dispose(disposing);
        }


    }

    [Android.Runtime.Preserve(AllMembers = true)]
    class SettingsViewSimpleCallback : ItemTouchHelper.SimpleCallback
    {
        SettingsView _settingsView;
        int _offset = 0;

        public SettingsViewSimpleCallback(SettingsView settingsView,int dragDirs,int swipeDirs):base(dragDirs,swipeDirs)
        {
            _settingsView = settingsView;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            var fromContentHolder = viewHolder as ContentViewHolder;
            if (fromContentHolder == null)
            {
                return false;
            }

            var toContentHolder = target as ContentViewHolder;
            if(toContentHolder == null){
                return false;
            }

            if (fromContentHolder.RowInfo.Section != toContentHolder.RowInfo.Section){
                return false;
            }

            var section = fromContentHolder.RowInfo.Section;
            if(section == null || !section.UseDragSort){
                return false;
            }

            var fromPos = viewHolder.AdapterPosition;
            var toPos = target.AdapterPosition;

            _offset += toPos - fromPos;

            var settingsAdapter = recyclerView.GetAdapter() as SettingsViewRecyclerAdapter;

            settingsAdapter.CellMoved(fromPos, toPos); //caches update
            settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update


            Console.WriteLine($"From:{fromPos} To:{toPos} Offset:{_offset}");

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

            var section = contentHolder.RowInfo.Section;
            var pos = section.IndexOf(contentHolder.RowInfo.Cell);

            if(section.ItemsSource == null){
                section.MoveCellWithoutNotify(pos, pos + _offset);            
            }
            else if(section.ItemsSource != null)
            {
                // must update DataSource at this timing.
                section.MoveSourceItemWithoutNotify(pos, pos + _offset);
            }

            _offset = 0;
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
