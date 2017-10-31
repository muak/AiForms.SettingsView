using System.Collections;
using System.Collections.Generic;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.App;

namespace AiForms.Renderers.Droid
{
    public class PickerAdapter:BaseAdapter<object>,AdapterView.IOnItemClickListener
    {
        Android.Content.Context _context;
        SettingsView _parent;
        PickerCell _pickerCell;
        ListView _listview;
        IList _source;
        bool _unLimited => _pickerCell.MaxSelectedNumber == 0;

        internal Color _accentColor;
        internal Color _titleColor;
        internal Color _background;
        internal double _fontSize;

        public PickerAdapter(Android.Content.Context context, PickerCell pickerCell,ListView listview)
        {
            _context = context;
            _listview = listview;
            _pickerCell = pickerCell;
            _parent = pickerCell.Parent as SettingsView;
            _source = pickerCell.ItemsSource as IList;

            if (pickerCell.SelectedItems == null)
            {
                pickerCell.SelectedItems = new List<object>();
            }

            if(_parent != null){
                _listview.SetBackgroundColor(_parent.BackgroundColor.ToAndroid());
                _listview.Divider = new ColorDrawable(_parent.SeparatorColor.ToAndroid());
                _listview.DividerHeight = 1;
            }

            SetUpProperties();
        }

        void SetUpProperties()
        {
            if (_pickerCell.AccentColor != Xamarin.Forms.Color.Default)
            {
                _accentColor = _pickerCell.AccentColor.ToAndroid();
            }
            else if (_parent.CellAccentColor != Xamarin.Forms.Color.Default)
            {
                _accentColor = _parent.CellAccentColor.ToAndroid();
            }

            if (_pickerCell.TitleColor != Xamarin.Forms.Color.Default)
            {
                _titleColor = _pickerCell.TitleColor.ToAndroid();
            }
            else if (_parent != null && _parent.CellTitleColor != Xamarin.Forms.Color.Default)
            {
                _titleColor = _parent.CellTitleColor.ToAndroid();
            }

            if (_pickerCell.TitleFontSize > 0)
            {
                _fontSize = _pickerCell.TitleFontSize;
            }
            else if (_parent != null)
            {
                _fontSize = _parent.CellTitleFontSize;
            }

            if (_pickerCell.BackgroundColor != Xamarin.Forms.Color.Default)
            {
                _background = _pickerCell.BackgroundColor.ToAndroid();
            }
            else if (_parent != null && _parent.CellBackgroundColor != Xamarin.Forms.Color.Default)
            {
                _background = _parent.CellBackgroundColor.ToAndroid();
            }
        }

        public void OnItemClick(AdapterView parent, AView view, int position, long id)
        {
            if(_listview.ChoiceMode == ChoiceMode.Single || _unLimited){
                return;
            }

            if (_listview.CheckedItemCount > _pickerCell.MaxSelectedNumber){              
                _listview.SetItemChecked(position, false);
            }
        }

        public void DoneSelect()
        {
            _pickerCell.SelectedItems.Clear();

            var positions = _listview.CheckedItemPositions;

            for (var i = 0; i < positions.Size(); i++)
            {
                if (!positions.ValueAt(i)) continue;

                var index = positions.KeyAt(i);
                _pickerCell.SelectedItems.Add(_source[index]);
            }
        }

        public void RestoreSelect()
        {
            if (_pickerCell.SelectedItems.Count == 0) {
                return;
            }

            for (var i = 0; i < _pickerCell.SelectedItems.Count;i++)
            {
                if (_pickerCell.MaxSelectedNumber >= 1 && i >= _pickerCell.MaxSelectedNumber)
                {
                    break;
                }

                var item = _pickerCell.SelectedItems[i];
                var pos = _source.IndexOf(item);
                if(pos < 0){
                    continue;
                }
                _listview.SetItemChecked(pos, true);
            }

            _listview.SetSelection(_listview.CheckedItemPositions.KeyAt(0));
        }

        public override object this[int position] => _source[position];

        public override int Count => _source.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override AView GetView(int position, AView convertView, ViewGroup parent)
        {
            if(convertView == null){
                convertView = new PickerInnerView(_context, this);
            }

            (convertView as PickerInnerView).UpdateCell(_pickerCell.DisplayValue(_source[position]));

            return convertView;
        }


        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _parent = null;
                _pickerCell = null;
                _source = null;
                _listview = null;
                _context = null;
            }
            base.Dispose(disposing);
        }
    }

    public class PickerInnerView: RelativeLayout,Android.Widget.ICheckable
    {
        TextView _textLabel;
        SimpleCheck _checkBox;
       
        public PickerInnerView(Android.Content.Context context,PickerAdapter adapter):base(context)
        {
            this.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            var padding = (int)context.ToPixels(8);
            SetPadding(padding,padding,padding,padding);

            SetBackgroundColor(adapter._background);

            _textLabel = new TextView(context);
            _textLabel.Id = AView.GenerateViewId();

            _checkBox = new SimpleCheck(context);
            _checkBox.Focusable = false;

            _textLabel.SetTextColor(adapter._titleColor);
            _textLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)adapter._fontSize);
            _checkBox.Color = adapter._accentColor;
            SetBackgroundColor(adapter._background);

            using (var param1 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent))
            {
                param1.AddRule(LayoutRules.AlignParentStart);
                param1.AddRule(LayoutRules.CenterVertical);
                AddView(_textLabel,param1);
            }

            using (var param2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent) {
                Width = (int)context.ToPixels(30),
                Height = (int)context.ToPixels(30)
            }){
                param2.AddRule(LayoutRules.AlignParentEnd);
                param2.AddRule(LayoutRules.CenterVertical);
                AddView(_checkBox,param2);
            }
        }

        public bool Checked { 
            get{
                return _checkBox.Selected;
            }
            set{
                _checkBox.Selected = value;
            }
        }
        public void Toggle(){
            _checkBox.Selected = !_checkBox.Selected;
        }

        public void UpdateCell(object displayValue)
        {
            _textLabel.Text = $"{displayValue}";
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _textLabel?.Dispose();
                _textLabel = null;
                _checkBox?.Dispose();
                _checkBox = null;
            }
            base.Dispose(disposing);
        }
    }

    public class SimpleCheck: AView
    {
        public Color Color { get; set; }
        Paint _paint = new Paint();
        Android.Content.Context _context;

        public SimpleCheck(Android.Content.Context context):base(context)
        {
            _context = context;
            SetWillNotDraw(false);
        }

        public override bool Selected {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                Invalidate();
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if(!base.Selected){
                return;
            }

            _paint.SetStyle(Paint.Style.Stroke);
            _paint.Color = Color;
            _paint.StrokeWidth = _context.ToPixels(2);
            _paint.AntiAlias = true;

            var fromX = 22f / 100f * canvas.Width;
            var fromY = 52f / 100f * canvas.Height;
            var toX = 38f / 100f * canvas.Width;
            var toY = 68f / 100f * canvas.Height;

            canvas.DrawLine(fromX, fromY, toX, toY,_paint);

            //fromX = toX;
            //fromY = toY;
            fromX = 36f / 100f * canvas.Width;
            fromY = 66f / 100f * canvas.Height;

            //toX = 76f / 100f * canvas.Width;
            //toY = 30f / 100f * canvas.Height;
            toX = 74f / 100f * canvas.Width;
            toY = 28f / 100f * canvas.Height;

            canvas.DrawLine(fromX, fromY,toX,toY,_paint);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _paint?.Dispose();
                _paint = null;
                _context = null;
            }
            base.Dispose(disposing);
        }
    }


}
