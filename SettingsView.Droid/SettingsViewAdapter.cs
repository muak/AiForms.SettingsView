using System;
using System.Collections.Generic;
using AiForms.Renderers.Droid.Extensions;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;
using AView = Android.Views.View;
using System.Linq;


namespace AiForms.Renderers.Droid
{
    public class SettingsViewAdapter : BaseAdapter<object>, AdapterView.IOnItemClickListener
    {
        enum ViewType
        {
            Header,
            Footer,
            Content
        }

        const int ViewTypeHeader = 0;
        const int ViewTypeFooter = 1;

        static readonly int CacheSize = (int)(Java.Lang.Runtime.GetRuntime().MaxMemory() / 1024 / 8);
        public MemoryLimitedLruCache ImageCache = new MemoryLimitedLruCache(CacheSize);

        Dictionary<Type, int> _viewTypes;

        float MinRowHeight => _context.ToPixels(44);
        SettingsView _settingsView;
        AListView _listView;
        Context _context;

        CellCache[] _cellCaches;
        CellCache[] CellCaches {
            get
            {
                if (_cellCaches == null)
                    FillCache();
                return _cellCaches;
            }
        }

        public SettingsViewAdapter(Context context, SettingsView settingsView, AListView listView)
        {
            _context = context;
            _settingsView = settingsView;
            _listView = listView;

            _listView.OnItemClickListener = this;
            _settingsView.ModelChanged += _settingsView_ModelChanged;
        }

        void _settingsView_ModelChanged(object sender, EventArgs e)
        {
            if (_listView != null)
            {
                _cellCaches = null;
                NotifyDataSetChanged();
            }
        }

        //Itemクリック AdapterView.IOnItemClickListenerに対応
        int _selectedIndex = -1;
        Android.Views.View _preSelectedCell = null;
        public void OnItemClick(AdapterView parent, AView view, int position, long id)
        {
            //TODO: 本当はForms側にSelectedを保持させておいてそれを反映するような処理が望ましい
            //      が、今はiOS側でそういう処理をしていないので後回し
            DeselectRow();

            var cell = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody).GetChildAt(0);

            //var cell = (view as ViewGroup)?.GetChildAt(0) ;

            _settingsView.Model.RowSelected(CellCaches[position].Cell);

            _preSelectedCell = cell;
            _selectedIndex = position;


            if(cell is CommandCellView){
                var cmdCell = cell as CommandCellView;
                cmdCell?.Execute?.Invoke();
                if ((cmdCell.Cell as CommandCell).KeepSelectedUntilBack)
                {
                    cell.Selected = true;
                }
            }
            else if (cell is IPickerCell)
            {
                var pCell = cell as IPickerCell;
                pCell.ShowDialog();
            }
            else if(cell is PickerCellView){
                var pCell = cell as PickerCellView;

                if((pCell.Cell as PickerCell).ItemsSource == null){
                    return;
                }

                if((pCell.Cell as PickerCell).KeepSelectedUntilBack){
                    cell.Selected = true;
                }
                pCell.ShowDialog();
            }

        }

        public void DeselectRow()
        {
            if (_preSelectedCell != null)
            {
                _preSelectedCell.Selected = false;
                _preSelectedCell = null;
            }
            _selectedIndex = -1;
        }

        //データソースのItemを返すインデクサ
        public override object this[int position] {
            get
            {
                return CellCaches[position];
            }
        }

        //Listの全行数
        public override int Count {
            get
            {
                return CellCaches.Length;
            }
        }

        //Idを返す（特に無いのでpositionを返しておく）
        public override long GetItemId(int position)
        {
            return position;
        }

        public override int ViewTypeCount {
            get
            {
                //used types count + header/footer
                return _viewTypes.Count() + 2;
            }
        }

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

        public override bool IsEnabled(int position)
        {
            var viewT = GetItemViewType(position);
            return viewT >= 2;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            var cellInfo = CellCaches[position];

            AView nativeCell;
            switch (GetItemViewType(position))
            {
                case ViewTypeHeader:
                    nativeCell = GetHeaderView(convertView, (TextCell)cellInfo.Cell);
                    break;
                case ViewTypeFooter:
                    nativeCell = GetFooterView(convertView, (TextCell)cellInfo.Cell);
                    break;
                default:
                    nativeCell = GetContentView(convertView, cellInfo.Cell, parent,position);
                    break;
            }

            return nativeCell;

        }

        AView GetHeaderView(AView convertView, TextCell formsCell)
        {
            TextView textView = null;

            if (convertView == null)
            {
                convertView = (_context as FormsAppCompatActivity).LayoutInflater.Inflate(Resource.Layout.HeaderCell, _listView, false);
            }

            //セルの高さ
            int cellHeight = (int)_context.ToPixels(44);
            if (_settingsView.HeaderHeight > -1)
            {
                cellHeight = (int)_context.ToPixels(_settingsView.HeaderHeight);
            }
            convertView.SetMinimumHeight(cellHeight);
            convertView.LayoutParameters.Height = cellHeight;

            textView = convertView.FindViewById<TextView>(Resource.Id.HeaderCellText);
            var border = convertView.FindViewById<LinearLayout>(Resource.Id.HeaderCellBorder);

            //Text設定
            textView.SetPadding(
                (int)_context.ToPixels(_settingsView.HeaderPadding.Left),
                (int)_context.ToPixels(_settingsView.HeaderPadding.Top),
                (int)_context.ToPixels(_settingsView.HeaderPadding.Right),
                (int)_context.ToPixels(_settingsView.HeaderPadding.Bottom)
            );

            textView.Gravity = _settingsView.HeaderTextVerticalAlign.ToNativeVertical() | GravityFlags.Left;
            textView.TextAlignment = Android.Views.TextAlignment.Gravity;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.HeaderFontSize);
            textView.SetBackgroundColor(_settingsView.HeaderBackgroundColor.ToAndroid());
            textView.SetMaxLines(1);
            textView.SetMinLines(1);
            textView.Ellipsize = TextUtils.TruncateAt.End;

            if (_settingsView.HeaderTextColor != Xamarin.Forms.Color.Default)
            {
                textView.SetTextColor(_settingsView.HeaderTextColor.ToAndroid());
            }

            //border設定
            if (_settingsView.ShowSectionTopBottomBorder)
            {
                border.SetBackgroundColor(_settingsView.SeparatorColor.ToAndroid());
            }
            else{
                border.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }


            //テキストの更新
            textView.Text = formsCell.Text;

            return convertView;
        }

        AView GetContentView(AView convertView, Cell formsCell, ViewGroup parent,int position)
        {
            AView nativeCell = null;
            AView layout = convertView as AView;

            if (layout == null)
            {
                layout = (_context as FormsAppCompatActivity).LayoutInflater.Inflate(Resource.Layout.ContentCell, _listView, false);
            }

            var body = layout.FindViewById<LinearLayout>(Resource.Id.ContentCellBody);
            var border = layout.FindViewById(Resource.Id.ContentCellBorder);

            nativeCell = body.GetChildAt(0);
            if(nativeCell != null){
                body.RemoveViewAt(0);
            }

            nativeCell = CellFactory.GetCell(formsCell, nativeCell, parent, _context, _settingsView);

            if(position == _selectedIndex){
                
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
                //Unevenでない場合は指定RowHeightかMinRowHeightの大きい方にする
                layout.LayoutParameters.Height = minHeight;
            }
            else if (formsCell.Height > -1)
            {
                //Cell自身にHeightが指定されて入ればそれを
                layout.SetMinimumHeight((int)_context.ToPixels(formsCell.Height));
                layout.LayoutParameters.Height = (int)_context.ToPixels(formsCell.Height);
            }
            else{
                layout.LayoutParameters.Height = -1;
            }

            if(!CellCaches[position].IsLastCell || _settingsView.ShowSectionTopBottomBorder){
               border.SetBackgroundColor(_settingsView.SeparatorColor.ToAndroid());
            }
            else{
                border.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

            body.AddView(nativeCell, 0);

            return layout;
        }

        AView GetFooterView(AView convertView, TextCell formsCell)
        {
            TextView textView = null;

            if (convertView == null)
            {
                convertView = (_context as FormsAppCompatActivity).LayoutInflater.Inflate(Resource.Layout.FooterCell, _listView, false);
            }

            textView = convertView.FindViewById<TextView>(Resource.Id.FooterCellText);

            //Text設定
            textView.SetPadding(
                (int)_context.ToPixels(_settingsView.FooterPadding.Left),
                (int)_context.ToPixels(_settingsView.FooterPadding.Top),
                (int)_context.ToPixels(_settingsView.FooterPadding.Right),
                (int)_context.ToPixels(_settingsView.FooterPadding.Bottom)
            );

            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_settingsView.FooterFontSize);
            textView.SetBackgroundColor(_settingsView.FooterBackgroundColor.ToAndroid());
            if (_settingsView.FooterTextColor != Xamarin.Forms.Color.Default)
            {
                textView.SetTextColor(_settingsView.FooterTextColor.ToAndroid());
            }


            //セルの高さ
            if (string.IsNullOrEmpty(formsCell.Text))
            {
                //テキスト未指定なら非表示（高さ0）
                textView.Visibility = ViewStates.Gone;
                convertView.Visibility = ViewStates.Gone;
            }
            else
            {
                textView.Visibility = ViewStates.Visible;
                convertView.Visibility = ViewStates.Visible;
            }

            //テキストの更新
            textView.Text = formsCell.Text;

            return convertView;
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

                Cell headerCell = new TextCell { Text = sectionTitle };
                headerCell.Parent = _settingsView;

                newCellCaches.Add(new CellCache {
                    Cell = headerCell,
                    IsHeader = true,
                });

                for (int i = 0; i < sectionRowCount; i++)
                {
                    newCellCaches.Add(new CellCache
                    {
                        Cell = (Cell)model.GetItem(sectionIndex, i),
                        IsLastCell = i == sectionRowCount - 1
                    });
                }

                Cell footerCell = new TextCell { Text = model.GetFooterText(sectionIndex) };
                footerCell.Parent = _settingsView;

                newCellCaches.Add(new CellCache {
                    Cell = footerCell,
                    IsFooter = true,
                });
            }

            _cellCaches = newCellCaches.ToArray();

            _viewTypes = _cellCaches.Select(x => x.Cell.GetType()).Distinct().Select((x, idx) => new { x, index = idx }).ToDictionary(key => key.x, val => val.index + 2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _settingsView.ModelChanged -= _settingsView_ModelChanged;
                _cellCaches = null;
                _settingsView = null;
                _viewTypes = null;
            }
            base.Dispose(disposing);
        }


        class CellCache
        {
            public Cell Cell { get; set; }
            public bool IsHeader { get; set; } = false;
            public bool IsFooter { get; set; } = false;
            public bool IsLastCell { get; set; } = false;
        }
    }
}
