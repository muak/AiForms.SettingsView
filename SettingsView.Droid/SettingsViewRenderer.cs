using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using System.Collections.Generic;
using System.Collections;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Settings view renderer.
    /// </summary>
    public class SettingsViewRenderer : ViewRenderer<SettingsView, RecyclerView>
    {
        Page _parentPage;
        SettingsViewRecyclerAdapter _adapter;
        LinearLayoutManager _layoutManager;
        ItemTouchHelper _itemTouchhelper;
        SettingsViewSimpleCallback _simpleCallback;

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

            if (e.NewElement != null) {

                var recyclerView = new RecyclerView(Context);
                _layoutManager = new LinearLayoutManager(Context);
                recyclerView.SetLayoutManager(_layoutManager);

                SetNativeControl(recyclerView);

                Control.Focusable = false;
                Control.DescendantFocusability = DescendantFocusability.AfterDescendants;

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

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.SeparatorColorProperty.PropertyName) {
                _adapter.NotifyDataSetChanged();
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
                _adapter.NotifyDataSetChanged();
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
            if (disposing) {
                _parentPage.Appearing -= ParentPageAppearing;
                _adapter?.Dispose();
                _adapter = null;
                _layoutManager?.Dispose();
                _layoutManager = null;
                _simpleCallback?.Dispose();
                _simpleCallback = null;
                _itemTouchhelper?.Dispose();
                _itemTouchhelper = null;
            }
            base.Dispose(disposing);
        }


    }

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

            if(fromContentHolder.SectionIndex != toContentHolder.SectionIndex){
                return false;
            }

            var section = _settingsView.Model.GetSection(fromContentHolder.SectionIndex);
            if(section == null || !section.UseDragSort){
                return false;
            }

            var fromPos = viewHolder.AdapterPosition;
            var toPos = target.AdapterPosition;

            _offset += toPos - fromPos;

            var settingsAdapter = recyclerView.GetAdapter() as SettingsViewRecyclerAdapter;

            settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update
            settingsAdapter.CellMoved(fromPos, toPos); //caches update

            //Console.WriteLine($"From:{fromPos} To:{toPos} Offset:{_offset}");

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

            var section = _settingsView.Model.GetSection(contentHolder.SectionIndex);

            if (section?.ItemsSource != null)
            {
                // must update DataSource at this timing.
                var pos = contentHolder.RowIndex;
                var tmp = section.ItemsSource[pos];
                section.ItemsSource.RemoveAt(pos);
                section.ItemsSource.Insert(pos + _offset, tmp);
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

            var section = _settingsView.Model.GetSection(contentHolder.SectionIndex);
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
