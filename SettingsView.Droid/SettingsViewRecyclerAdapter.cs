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
    public class SettingsViewRecyclerAdapter:RecyclerView.Adapter,AView.IOnClickListener, AView.IOnLongClickListener
    {
        float MinRowHeight => _context.ToPixels(44);

        //Item click. correspond to AdapterView.IOnItemClickListener
        int _selectedIndex = -1;
        Android.Views.View _preSelectedCell = null;

        Context _context;
        SettingsView _settingsView;
        RecyclerView _recyclerView;
        ModelProxy _proxy;

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
            _proxy = new ModelProxy(settingsView, this);

            _settingsView.ModelChanged += _settingsView_ModelChanged;
            _settingsView.SectionPropertyChanged += OnSectionPropertyChanged;
        }

        void _settingsView_ModelChanged(object sender, EventArgs e)
        {
            if (_recyclerView != null)
            {
                _proxy.FillProxy();
                NotifyDataSetChanged();
            }
        }

        void OnSectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Section.IsVisibleProperty.PropertyName)
            {
                UpdateSectionVisible((Section)sender);
            }
            else if (e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
                     e.PropertyName == Section.HeaderViewProperty.PropertyName ||
                     e.PropertyName == Section.HeaderHeightProperty.PropertyName)
            {
                UpdateSectionHeader((Section)sender);
            }
            else if (e.PropertyName == Section.FooterTextProperty.PropertyName ||
                     e.PropertyName == Section.FooterViewProperty.PropertyName)
            {
                UpdateSectionFooter((Section)sender);
            }
        }

        void UpdateSectionVisible(Section section)
        {
            var indexes = _proxy.Select((x, idx) => new { idx, x.Section }).Where(x => x.Section == section).Select(x => x.idx).ToList();
            NotifyItemRangeChanged(indexes[0], indexes.Count);
        }

        void UpdateSectionHeader(Section section)
        {
            var index = _proxy.FindIndex(x => x.Section == section);
            NotifyItemChanged(index);
        }

        void UpdateSectionFooter(Section section)
        {
            var index = _proxy.FindLastIndex(x => x.Section == section);
            NotifyItemChanged(index);
        }


        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount => _proxy.Count;

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
        /// Gets the type of the item view.
        /// </summary>
        /// <returns>The item view type.</returns>
        /// <param name="position">Position.</param>
        public override int GetItemViewType(int position)
        {
            return (int)_proxy[position].ViewType;
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

            var inflater = LayoutInflater.FromContext(_context);

            switch ((ViewType)viewType)
            {
                case ViewType.TextHeader:
                    viewHolder = new HeaderViewHolder(inflater.Inflate(Resource.Layout.HeaderCell, parent, false));
                    break;
                case ViewType.TextFooter:
                    viewHolder = new FooterViewHolder(inflater.Inflate(Resource.Layout.FooterCell, parent, false));
                    break;
                case ViewType.CustomHeader:
                    var hContainer = new HeaderFooterContainer(_context);
                    viewHolder = new CustomHeaderViewHolder(hContainer);
                    break;
                case ViewType.CustomFooter:
                    var fContainer = new HeaderFooterContainer(_context);
                    viewHolder = new CustomFooterViewHolder(fContainer);
                    break;
                default:
                    viewHolder = new ContentViewHolder(inflater.Inflate(Resource.Layout.ContentCell, parent, false));
                    viewHolder.ItemView.SetOnClickListener(this);
                    viewHolder.ItemView.SetOnLongClickListener(this);
                    break;
            }

            _viewHolders.Add(viewHolder);

            inflater.Dispose();

            return viewHolder;
        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var rowInfo = _proxy[position];

            var vHolder = holder as ViewHolder;
            vHolder.RowInfo = rowInfo;
            
            if(!rowInfo.Section.IsVisible)
            {
                vHolder.ItemView.Visibility = ViewStates.Gone;
                vHolder.ItemView.SetMinimumHeight(0);
                vHolder.ItemView.LayoutParameters.Height = 0;
                return;
            }

            vHolder.ItemView.Visibility = ViewStates.Visible;

            switch (rowInfo.ViewType)
            {
                case ViewType.TextHeader:
                    BindHeaderView((HeaderViewHolder)vHolder);
                    break;
                case ViewType.TextFooter:
                    BindFooterView((FooterViewHolder)vHolder);
                    break;
                case ViewType.CustomHeader:
                    BindCustomHeaderFooterView(vHolder, rowInfo.Section.HeaderView);
                    break;
                case ViewType.CustomFooter:
                    BindCustomHeaderFooterView(vHolder, rowInfo.Section.FooterView);
                    break;
                default:
                    BindContentView((ContentViewHolder)vHolder, position);
                    break;
            }
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

            if(cell == null || !_proxy[position].Cell.IsEnabled){
                //if FormsCell IsEnable is false, does nothing. 
                return;
            }

            _settingsView.Model.RowSelected(_proxy[position].Cell);

            cell.RowSelected(this,position);
        }

        public virtual bool OnLongClick(AView v)
        {
            var position = _recyclerView.GetChildAdapterPosition(v);

            DeselectRow();

            if(_proxy[position].Section.UseDragSort)
            {
                return false;
            }

            var cell = v.FindViewById<LinearLayout>(Resource.Id.ContentCellBody).GetChildAt(0) as CellBaseView;

            if (cell == null || !_proxy[position].Cell.IsEnabled)
            {
                //if FormsCell IsEnable is false, does nothing. 
                return false;
            }

            _settingsView.Model.RowLongPressed(_proxy[position].Cell);

            cell.RowLongPressed(this, position);

            return true;
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
                _settingsView.SectionPropertyChanged -= OnSectionPropertyChanged;
                _proxy?.Dispose();
                _proxy = null;
                _settingsView = null;

                foreach (var holder in _viewHolders)
                {
                    holder.Dispose();
                }
                _viewHolders.Clear();
                _viewHolders = null;
            }
            base.Dispose(disposing);
        }

       
        void BindHeaderView(HeaderViewHolder holder)
        {
            var section = holder.RowInfo.Section;
            var view = holder.ItemView;

            //judging cell height
            int cellHeight = (int)_context.ToPixels(44);
            var individualHeight = section.HeaderHeight;

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
            holder.TextView.Typeface = FontUtility.CreateTypeface(_settingsView.HeaderFontFamily, _settingsView.HeaderFontAttributes);
            holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.HeaderFontSize);
            holder.TextView.SetBackgroundColor(_settingsView.HeaderBackgroundColor.ToAndroid());
            holder.TextView.SetMaxLines(1);
            holder.TextView.SetMinLines(1);
            holder.TextView.Ellipsize = TextUtils.TruncateAt.End;

            if (_settingsView.HeaderTextColor != Xamarin.Forms.Color.Default)
            {
                holder.TextView.SetTextColor(_settingsView.HeaderTextColor.ToAndroid());
            }

            //update text
            holder.TextView.Text = section.Title;
        }

        void BindFooterView(FooterViewHolder holder)
        {
            var section = holder.RowInfo.Section;
            var view = holder.ItemView;

            //footer visible setting
            if (string.IsNullOrEmpty(section.FooterText))
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

            holder.TextView.Typeface = FontUtility.CreateTypeface(_settingsView.FooterFontFamily, _settingsView.FooterFontAttributes);
            holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.FooterFontSize);
            holder.TextView.SetBackgroundColor(_settingsView.FooterBackgroundColor.ToAndroid());
            if (_settingsView.FooterTextColor != Xamarin.Forms.Color.Default)
            {
                holder.TextView.SetTextColor(_settingsView.FooterTextColor.ToAndroid());
            }

            //update text
            holder.TextView.Text = section.FooterText;
        }

        void BindCustomHeaderFooterView(ViewHolder holder, Xamarin.Forms.View formsView)
        {
            var nativeCell = holder.ItemView as HeaderFooterContainer;
            nativeCell.FormsCell = formsView;
        }


        void BindContentView(ContentViewHolder holder, int position)
        {
            var formsCell = holder.RowInfo.Cell;
            AView nativeCell = null;
            AView layout = holder.ItemView;

            holder.RowInfo = _proxy[position];

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
                // if not Uneven, set the larger one of RowHeight and MinRowHeight.
                layout.LayoutParameters.Height = minHeight;
            }
            else if (formsCell.Height > -1)
            {
                // if the cell itself was specified height, set it.
                layout.SetMinimumHeight((int)_context.ToPixels(formsCell.Height));
                layout.LayoutParameters.Height = (int)_context.ToPixels(formsCell.Height);
            }
            else if (formsCell is ViewCell viewCell) 
            {
                // if used a viewcell, calculate the size and layout it.
                var size = viewCell.View.Measure(_settingsView.Width, double.PositiveInfinity);
                viewCell.View.Layout(new Rectangle(0, 0, size.Request.Width, size.Request.Height));
                layout.LayoutParameters.Height = (int)_context.ToPixels(size.Request.Height);
            }
            else
            {
                layout.LayoutParameters.Height = -2; //wrap_content
            }

            holder.Body.AddView(nativeCell, 0);
        }


        /// <summary>
        /// Cells the moved.
        /// </summary>
        /// <param name="fromPos">From position.</param>
        /// <param name="toPos">To position.</param>
        public void CellMoved(int fromPos,int toPos)
        {
            var tmp = _proxy[fromPos];
            _proxy.RemoveAt(fromPos);
            _proxy.Insert(toPos,tmp);
        }

    }


}
