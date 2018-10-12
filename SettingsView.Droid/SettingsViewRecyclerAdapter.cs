using System;
using System.Collections.Generic;
using System.Linq;
using AiForms.Renderers.Droid.Extensions;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Settings view recycler adapter.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SettingsViewRecyclerAdapter:RecyclerView.Adapter,AView.IOnClickListener
    {
        const int ViewTypeHeader = 0;
        const int ViewTypeFooter = 1;

        float MinRowHeight => _context.ToPixels(44);

        Dictionary<Type, int> _viewTypes;
        List<CellCache> _cellCaches;
        internal List<CellCache> CellCaches
        {
            get
            {
                if (_cellCaches == null)
                    FillCache();
                return _cellCaches;
            }
        }

        //Item click. correspond to AdapterView.IOnItemClickListener
        int _selectedIndex = -1;
        Android.Views.View _preSelectedCell = null;

        Context _context;
        SettingsView _settingsView;
        RecyclerView _recyclerView;

        List<ViewHolder> _viewHolders = new List<ViewHolder>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SettingsViewRecyclerAdapter"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="settingsView">Settings view.</param>
        /// <param name="recyclerView">Recycler view.</param>
        public SettingsViewRecyclerAdapter(Context context, SettingsView settingsView,RecyclerView recyclerView)
        {
            _context = context;
            _settingsView = settingsView;
            _recyclerView = recyclerView;

            _settingsView.ModelChanged += _settingsView_ModelChanged;
        }

        void _settingsView_ModelChanged(object sender, EventArgs e)
        {
            if (_recyclerView != null)
            {
                _cellCaches = null;
                NotifyDataSetChanged();
            }
        }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount => CellCaches.Count;

        /// <summary>
        /// return ID (As in paticular it doesn't exist, return the position.)
        /// </summary>
        /// <returns>The item identifier.</returns>
        /// <param name="position">Position.</param>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Ons the create view holder.
        /// </summary>
        /// <returns>The create view holder.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="viewType">View type.</param>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            ViewHolder viewHolder;

            switch (viewType)
            {
                case ViewTypeHeader:
                    viewHolder = new HeaderViewHolder(LayoutInflater.FromContext(_context).Inflate(Resource.Layout.HeaderCell, parent, false),_settingsView);
                    break;
                case ViewTypeFooter:
                    viewHolder = new FooterViewHolder(LayoutInflater.FromContext(_context).Inflate(Resource.Layout.FooterCell, parent, false),_settingsView);
                    break;
                default:
                    viewHolder = new ContentViewHolder(LayoutInflater.FromContext(_context).Inflate(Resource.Layout.ContentCell, parent, false));
                    viewHolder.ItemView.SetOnClickListener(this);
                    break;
            }

            _viewHolders.Add(viewHolder);

            return viewHolder;
        }

        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="view">View.</param>
        public void OnClick(AView view)
        {
            var position = _recyclerView.GetChildAdapterPosition(view);

            //TODO: It is desirable that the forms side has Selected property and reflects it.
            //      But do it at a later as iOS side doesn't have that process.
            DeselectRow();

            var cell = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody).GetChildAt(0) as CellBaseView;


            if(cell == null || !CellCaches[position].Cell.IsEnabled){
                //if FormsCell IsEnable is false, does nothing. 
                return;
            }

            _settingsView.Model.RowSelected(CellCaches[position].Cell);

            cell.RowSelected(this,position);

            //if (cell is CommandCellView){
            //    var cmdCell = cell as CommandCellView;
            //    cmdCell?.Execute?.Invoke();
            //    if ((cmdCell.Cell as CommandCell).KeepSelectedUntilBack)
            //    {
            //        SelectedRow(cell, position);
            //    }
            //}
            //else if (cell is ButtonCellView)
            //{
            //    var buttonCell = cell as ButtonCellView;
            //    buttonCell?.Execute?.Invoke();
            //}
            //else if (cell is ICheckableCell)
            //{
            //    var checkCell = cell as ICheckableCell;
            //    checkCell.CheckChange();
            //}
            //else if (cell is IPickerCell)
            //{
            //    var pCell = cell as IPickerCell;
            //    pCell.ShowDialog();
            //}
            //else if (cell is PickerCellView)
            //{
            //    var pCell = cell as PickerCellView;
            //    var formPickerCell = pCell.Cell as PickerCell;

            //    if (formPickerCell.ItemsSource == null){
            //        return;
            //    }

            //    if(formPickerCell.ItemsSource.Count == 0){
            //        return;
            //    }

            //    if (formPickerCell.KeepSelectedUntilBack){
            //        SelectedRow(cell, position);
            //    }
            //    pCell.ShowDialog();
            //}

        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var cellInfo = CellCaches[position];

            switch(holder.ItemViewType)
            {
                case ViewTypeHeader:
                    BindHeaderView((HeaderViewHolder)holder, (TextCell)cellInfo.Cell);
                    break;
                case ViewTypeFooter:
                    BindFooterView((FooterViewHolder)holder, (TextCell)cellInfo.Cell);
                    break;
                default:
                    BindContentView((ContentViewHolder)holder, cellInfo.Cell, position);
                    break;
            }
        }

        /// <summary>
        /// Gets the type of the item view.
        /// </summary>
        /// <returns>The item view type.</returns>
        /// <param name="position">Position.</param>
        public override int GetItemViewType(int position)
        {
            var cellInfo = CellCaches[position];
            if (cellInfo.IsHeader)
            {
                return ViewTypeHeader;
            }
            else if (cellInfo.IsFooter)
            {
                return ViewTypeFooter;
            }
            else
            {
                return _viewTypes[cellInfo.Cell.GetType()];
            }
        }

        /// <summary>
        /// Deselects the row.
        /// </summary>
        public void DeselectRow()
        {
            if (_preSelectedCell != null)
            {
                _preSelectedCell.Selected = false;
                _preSelectedCell = null;
            }
            _selectedIndex = -1;
        }

        /// <summary>
        /// Selecteds the row.
        /// </summary>
        /// <param name="cell">Cell.</param>
        /// <param name="position">Position.</param>
        public void SelectedRow(AView cell, int position)
        {
            _preSelectedCell = cell;
            _selectedIndex = position;
            cell.Selected = true;
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _settingsView.ModelChanged -= _settingsView_ModelChanged;
                _cellCaches?.Clear();
                _cellCaches = null;
                _settingsView = null;
                _viewTypes = null;

                foreach (var holder in _viewHolders)
                {
                    holder.Dispose();
                }
                _viewHolders.Clear();
                _viewHolders = null;
            }
            base.Dispose(disposing);
        }

       
        void BindHeaderView(HeaderViewHolder holder, TextCell formsCell)
        {
            var view = holder.ItemView;

            //judging cell height
            int cellHeight = (int)_context.ToPixels(44);
            var individualHeight = formsCell.Height;

            if (individualHeight > 0d)
            {
                cellHeight = (int)_context.ToPixels(individualHeight);
            }
            else if (_settingsView.HeaderHeight > -1)
            {
                cellHeight = (int)_context.ToPixels(_settingsView.HeaderHeight);
            }

            view.SetMinimumHeight(cellHeight);
            view.LayoutParameters.Height = cellHeight;

            //textview setting
            holder.TextView.SetPadding(
                (int)view.Context.ToPixels(_settingsView.HeaderPadding.Left),
                (int)view.Context.ToPixels(_settingsView.HeaderPadding.Top),
                (int)view.Context.ToPixels(_settingsView.HeaderPadding.Right),
                (int)view.Context.ToPixels(_settingsView.HeaderPadding.Bottom)
            );

            holder.TextView.Gravity = _settingsView.HeaderTextVerticalAlign.ToNativeVertical() | GravityFlags.Left;
            holder.TextView.TextAlignment = Android.Views.TextAlignment.Gravity;
            holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.HeaderFontSize);
            holder.TextView.SetBackgroundColor(_settingsView.HeaderBackgroundColor.ToAndroid());
            holder.TextView.SetMaxLines(1);
            holder.TextView.SetMinLines(1);
            holder.TextView.Ellipsize = TextUtils.TruncateAt.End;

            if (_settingsView.HeaderTextColor != Xamarin.Forms.Color.Default)
            {
                holder.TextView.SetTextColor(_settingsView.HeaderTextColor.ToAndroid());
            }

            //border setting
            if (_settingsView.ShowSectionTopBottomBorder)
            {
                holder.Border.SetBackgroundColor(_settingsView.SeparatorColor.ToAndroid());
            }
            else
            {
                holder.Border.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

            //update text
            holder.TextView.Text = formsCell.Text;


        }

        void BindFooterView(FooterViewHolder holder, TextCell formsCell)
        {
            var view = holder.ItemView;

            //footer visible setting
            if (string.IsNullOrEmpty(formsCell.Text))
            {
                //if text is empty, hidden (height 0)
                holder.TextView.Visibility = ViewStates.Gone;
                view.Visibility = ViewStates.Gone;
            }
            else
            {
                holder.TextView.Visibility = ViewStates.Visible;
                view.Visibility = ViewStates.Visible;
            }

            //textview setting
            holder.TextView.SetPadding(
                (int)view.Context.ToPixels(_settingsView.FooterPadding.Left),
                (int)view.Context.ToPixels(_settingsView.FooterPadding.Top),
                (int)view.Context.ToPixels(_settingsView.FooterPadding.Right),
                (int)view.Context.ToPixels(_settingsView.FooterPadding.Bottom)
            );

            holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.FooterFontSize);
            holder.TextView.SetBackgroundColor(_settingsView.FooterBackgroundColor.ToAndroid());
            if (_settingsView.FooterTextColor != Xamarin.Forms.Color.Default)
            {
                holder.TextView.SetTextColor(_settingsView.FooterTextColor.ToAndroid());
            }

            //update text
            holder.TextView.Text = formsCell.Text;
        }

        void BindContentView(ContentViewHolder holder, Cell formsCell, int position)
        {
            AView nativeCell = null;
            AView layout = holder.ItemView;

            holder.SectionIndex = CellCaches[position].SectionIndex;
            holder.RowIndex = CellCaches[position].RowIndex;

            nativeCell = holder.Body.GetChildAt(0);
            if (nativeCell != null)
            {
                holder.Body.RemoveViewAt(0);
            }

            nativeCell = CellFactory.GetCell(formsCell, nativeCell, _recyclerView, _context, _settingsView);

            if (position == _selectedIndex)
            {

                DeselectRow();
                nativeCell.Selected = true;

                _preSelectedCell = nativeCell;
            }

            var minHeight = (int)Math.Max(_context.ToPixels(_settingsView.RowHeight), MinRowHeight);

            //it is neccesary to set both
            layout.SetMinimumHeight(minHeight);
            nativeCell.SetMinimumHeight(minHeight);

            if (!_settingsView.HasUnevenRows)
            {
                //if not Uneven, set the larger one of RowHeight and MinRowHeight.
                layout.LayoutParameters.Height = minHeight;
            }
            else if (formsCell.Height > -1)
            {
                //if the cell itself was specified height, set it.
                layout.SetMinimumHeight((int)_context.ToPixels(formsCell.Height));
                layout.LayoutParameters.Height = (int)_context.ToPixels(formsCell.Height);
            }
            else
            {
                layout.LayoutParameters.Height = -2; //wrap_content
            }

            if (!CellCaches[position].IsLastCell || _settingsView.ShowSectionTopBottomBorder)
            {
                holder.Border.SetBackgroundColor(_settingsView.SeparatorColor.ToAndroid());
            }
            else
            {
                holder.Border.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

            holder.Body.AddView(nativeCell, 0);
           
        }

        void FillCache()
        {
            SettingsModel model = _settingsView.Model;
            int sectionCount = model.GetSectionCount();

            var newCellCaches = new List<CellCache>();

            for (var sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
            {
                var sectionTitle = model.GetSectionTitle(sectionIndex);
                var sectionRowCount = model.GetRowCount(sectionIndex);

                Cell headerCell = new TextCell { Text = sectionTitle, Height = model.GetHeaderHeight(sectionIndex) };
                headerCell.Parent = _settingsView;

                newCellCaches.Add(new CellCache
                {
                    Cell = headerCell,
                    IsHeader = true,
                    SectionIndex = sectionIndex,
                });

                for (int i = 0; i < sectionRowCount; i++)
                {
                    newCellCaches.Add(new CellCache
                    {
                        Cell = model.GetCell(sectionIndex, i),
                        IsLastCell = i == sectionRowCount - 1,
                        SectionIndex = sectionIndex,
                        RowIndex = i
                    });
                }

                Cell footerCell = new TextCell { Text = model.GetFooterText(sectionIndex) };
                footerCell.Parent = _settingsView;

                newCellCaches.Add(new CellCache
                {
                    Cell = footerCell,
                    IsFooter = true,
                    SectionIndex = sectionIndex,
                });
            }

            _cellCaches = newCellCaches;

            if(_viewTypes == null)
            {
                _viewTypes = _cellCaches.Select(x => x.Cell.GetType()).Distinct().Select((x, idx) => new { x, index = idx }).ToDictionary(key => key.x, val => val.index + 2);
            }
            else
            {
                var idx = _viewTypes.Values.Max() + 1;
                foreach(var t in _cellCaches.Select(x=>x.Cell.GetType()).Distinct().Except(_viewTypes.Keys).ToList())
                {
                    _viewTypes.Add(t, idx++);
                }
            }
        }

        /// <summary>
        /// Cells the moved.
        /// </summary>
        /// <param name="fromPos">From position.</param>
        /// <param name="toPos">To position.</param>
        public void CellMoved(int fromPos,int toPos)
        {
            var tmp = CellCaches[fromPos];
            CellCaches.RemoveAt(fromPos);
            CellCaches.Insert(toPos,tmp);
        }
 

        [Android.Runtime.Preserve(AllMembers = true)]
        internal class CellCache
        {
            public Cell Cell { get; set; }
            public bool IsHeader { get; set; } = false;
            public bool IsFooter { get; set; } = false;
            public bool IsLastCell { get; set; } = false;
            public int SectionIndex { get; set; }
            public int RowIndex { get; set; }
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ViewHolder : RecyclerView.ViewHolder
    {
        public ViewHolder(AView view) : base(view) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemView?.Dispose();
                ItemView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class HeaderViewHolder : ViewHolder
    {
        public TextView TextView { get; private set; }
        public LinearLayout Border { get; private set; }

        public HeaderViewHolder(AView view, SettingsView settingsView) : base(view)
        {
            TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellText);
            Border = view.FindViewById<LinearLayout>(Resource.Id.HeaderCellBorder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TextView?.Dispose();
                TextView = null;
                Border?.Dispose();
                Border = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class FooterViewHolder : ViewHolder
    {
        public TextView TextView { get; private set; }

        public FooterViewHolder(AView view, SettingsView settingsView) : base(view)
        {
            TextView = view.FindViewById<TextView>(Resource.Id.FooterCellText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TextView?.Dispose();
                TextView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ContentViewHolder : ViewHolder
    {
        public LinearLayout Body { get; private set; }
        public AView Border { get; private set; }
        public int SectionIndex { get; set; }
        public int RowIndex { get; set; }

        public ContentViewHolder(AView view) : base(view)
        {
            Body = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody);
            Border = view.FindViewById(Resource.Id.ContentCellBorder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var nativeCell = Body.GetChildAt(0);
                nativeCell?.Dispose();
                Border?.Dispose();
                Border = null;
                Body?.Dispose();
                Body = null;
                ItemView.SetOnClickListener(null);
            }
            base.Dispose(disposing);
        }
    }
}
