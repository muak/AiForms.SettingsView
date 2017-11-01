using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;

namespace AiForms.Renderers.Droid
{
    public class CellBaseView : ARelativeLayout, INativeElementView
    {
        public Cell Cell { get; set; }
        public Element Element => Cell;

        protected CellBase CellBase => Cell as CellBase;
        public SettingsView CellParent => Cell.Parent as SettingsView;

        public TextView TitleLabel { get; set; }
        public TextView DescriptionLabel { get; set; }
        public ImageView IconView { get; set; }
        public LinearLayout ContentStack { get; set; }
        public LinearLayout AccessoryStack { get; set; }
        public TextView HintLabel { get; private set; }

        Context _context;
        CancellationTokenSource _iconTokenSource;
        Android.Graphics.Color _defaultTextColor;
        ColorDrawable _backgroundColor;
        ColorDrawable _selectedColor;
        float _defaultFontSize;
        float _iconRadius;

        public CellBaseView(Context context, Cell cell) : base(context)
        {
            _context = context;
            Cell = cell;

            CreateContentView();
        }

        void CreateContentView()
        {
            var contentView = (_context as FormsAppCompatActivity).LayoutInflater.Inflate(Resource.Layout.CellBaseView, this, true);

            contentView.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);

            IconView = contentView.FindViewById<ImageView>(Resource.Id.CellIcon);
            TitleLabel = contentView.FindViewById<TextView>(Resource.Id.CellTitle);
            DescriptionLabel = contentView.FindViewById<TextView>(Resource.Id.CellDescription);
            ContentStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellContentStack);
            AccessoryStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryView);
            HintLabel = contentView.FindViewById<TextView>(Resource.Id.CellHintText);

            _backgroundColor = new ColorDrawable();
            _selectedColor = new ColorDrawable(Android.Graphics.Color.Argb(125, 180, 180, 180));

            var sel = new StateListDrawable();

            sel.AddState(new int[] { global::Android.Resource.Attribute.StateSelected }, _selectedColor);
            sel.AddState(new int[] { -global::Android.Resource.Attribute.StateSelected }, _backgroundColor);
            sel.SetExitFadeDuration(250);
            sel.SetEnterFadeDuration(250);
            Background = sel;

            _defaultTextColor = new Android.Graphics.Color(TitleLabel.CurrentTextColor);
            _defaultFontSize = TitleLabel.TextSize;
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CellBase.TitleProperty.PropertyName) {
                UpdateTitleText();
            }
            else if (e.PropertyName == CellBase.TitleColorProperty.PropertyName) {
                UpdateTitleColor();
            }
            else if (e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName) {
                UpdateTitleFontSize();
            }
            else if (e.PropertyName == CellBase.DescriptionProperty.PropertyName) {
                UpdateDescriptionText();
            }
            else if (e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName) {
                UpdateDescriptionFontSize();
            }
            else if (e.PropertyName == CellBase.DescriptionColorProperty.PropertyName) {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == CellBase.IconSourceProperty.PropertyName) {
                UpdateIcon();
            }
            else if (e.PropertyName == CellBase.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == CellBase.HintTextProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintText);
            }
            else if (e.PropertyName == CellBase.HintTextColorProperty.PropertyName) {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == CellBase.HintFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if (e.PropertyName == CellBase.IconSizeProperty.PropertyName) {
                UpdateIcon();
            }
            else if (e.PropertyName == CellBase.IconRadiusProperty.PropertyName) {
                UpdateIconRadius();
                UpdateIcon(true);
            }
        }

        public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName) {
                UpdateTitleColor();
            }
            else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName) {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == SettingsView.CellHintTextColorProperty.PropertyName) {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if (e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName) {
                UpdateIcon();
            }
            else if (e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName) {
                UpdateIconRadius();
                UpdateIcon(true);
            }
            else if (e.PropertyName == SettingsView.SelectedColorProperty.PropertyName) {
                UpdateSelectedColor();
            }
        }

        protected void UpdateWithForceLayout(System.Action updateAction)
        {
            updateAction();
            Invalidate();
        }

        public virtual void UpdateCell()
        {
            UpdateBackgroundColor();
            UpdateSelectedColor();
            UpdateTitleText();
            UpdateTitleColor();
            UpdateTitleFontSize();
            UpdateDescriptionText();
            UpdateDescriptionColor();
            UpdateDescriptionFontSize();
            UpdateHintText();
            UpdateHintTextColor();
            UpdateHintFontSize();

            UpdateIcon();
            UpdateIconRadius();

            Invalidate();
        }

        void UpdateBackgroundColor()
        {
            Selected = false;

            if (CellBase.BackgroundColor != Xamarin.Forms.Color.Default) {
                _backgroundColor.Color = CellBase.BackgroundColor.ToAndroid();
            }
            else if (CellParent != null && CellParent.CellBackgroundColor != Xamarin.Forms.Color.Default) {
                _backgroundColor.Color = CellParent.CellBackgroundColor.ToAndroid();
            }
            else {
                _backgroundColor.Color = Android.Graphics.Color.Transparent;
            }
        }

        void UpdateSelectedColor()
        {
            if (CellParent != null && CellParent.SelectedColor != Xamarin.Forms.Color.Default) {
                _selectedColor.Color = CellParent.SelectedColor.MultiplyAlpha(0.5).ToAndroid();
            }
            else {
                _selectedColor.Color = Android.Graphics.Color.Argb(125, 180, 180, 180);
            }
        }

        void UpdateTitleText()
        {
            TitleLabel.Text = CellBase.Title;
            //hide TextView right padding when TextView.Text empty.
            TitleLabel.Visibility = string.IsNullOrEmpty(TitleLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
        }

        void UpdateTitleColor()
        {
            if (CellBase.TitleColor != Xamarin.Forms.Color.Default) {
                TitleLabel.SetTextColor(CellBase.TitleColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellTitleColor != Xamarin.Forms.Color.Default) {
                TitleLabel.SetTextColor(CellParent.CellTitleColor.ToAndroid());
            }
            else {
                TitleLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateTitleFontSize()
        {
            if (CellBase.TitleFontSize > 0) {
                TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.TitleFontSize);
            }
            else if (CellParent != null) {
                TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellTitleFontSize);
            }
            else {
                TitleLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
            }
        }

        void UpdateDescriptionText()
        {
            DescriptionLabel.Text = CellBase.Description;
            DescriptionLabel.Visibility = string.IsNullOrEmpty(DescriptionLabel.Text) ?
                ViewStates.Gone : ViewStates.Visible;
        }

        void UpdateDescriptionFontSize()
        {
            if (CellBase.DescriptionFontSize > 0) {
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.DescriptionFontSize);
            }
            else if (CellParent != null) {
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellDescriptionFontSize);
            }
            else {
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
            }
        }

        void UpdateDescriptionColor()
        {
            if (CellBase.DescriptionColor != Xamarin.Forms.Color.Default) {
                DescriptionLabel.SetTextColor(CellBase.DescriptionColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellDescriptionColor != Xamarin.Forms.Color.Default) {
                DescriptionLabel.SetTextColor(CellParent.CellDescriptionColor.ToAndroid());
            }
            else {
                DescriptionLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateHintText()
        {
            var msg = CellBase.HintText;
            if (string.IsNullOrEmpty(msg)) {
                HintLabel.Visibility = ViewStates.Gone;
                return;
            }

            HintLabel.Text = msg;
            HintLabel.Visibility = ViewStates.Visible;
        }

        void UpdateHintTextColor()
        {
            if (CellBase.HintTextColor != Xamarin.Forms.Color.Default) {
                HintLabel.SetTextColor(CellBase.HintTextColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellHintTextColor != Xamarin.Forms.Color.Default) {
                HintLabel.SetTextColor(CellParent.CellHintTextColor.ToAndroid());
            }
            else {
                HintLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateHintFontSize()
        {
            if (CellBase.HintFontSize > 0) {
                HintLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.HintFontSize);
            }
            else if (CellParent != null) {
                HintLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellHintFontSize);
            }
            else {
                HintLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
            }
        }

        void UpdateIconRadius()
        {
            if (CellBase.IconRadius >= 0) {
                _iconRadius = _context.ToPixels(CellBase.IconRadius);
            }
            else if (CellParent != null) {
                _iconRadius = _context.ToPixels(CellParent.CellIconRadius);
            }
        }

        void UpdateIconSize()
        {
            Xamarin.Forms.Size size;
            if (CellBase.IconSize != default(Xamarin.Forms.Size)) {
                size = CellBase.IconSize;
            }
            else if (CellParent != null && CellParent.CellIconSize != default(Xamarin.Forms.Size)) {
                size = CellParent.CellIconSize;
            }
            else {
                size = new Xamarin.Forms.Size(36, 36);
            }

            IconView.LayoutParameters.Width = (int)_context.ToPixels(size.Width);
            IconView.LayoutParameters.Height = (int)_context.ToPixels(size.Height);
        }

        void UpdateIcon(bool forceLoad = false)
        {

            if (_iconTokenSource != null && !_iconTokenSource.IsCancellationRequested) {
                //if previous task be alive, cancel. 
                _iconTokenSource.Cancel();
            }

            UpdateIconSize();

            if (IconView.Drawable != null) {
                IconView.SetImageDrawable(null);
                IconView.SetImageBitmap(null);
            }

            if (CellBase.IconSource != null) {
                IconView.Visibility = ViewStates.Visible;
                var cache = ImageCacheController.Instance.Get(CellBase.IconSource.GetHashCode()) as Bitmap;
                if (cache != null && !forceLoad) {
                    IconView.SetImageBitmap(cache);
                    Invalidate();
                    return;
                }

                var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(CellBase.IconSource.GetType());
                LoadIconImage(handler, CellBase.IconSource);
            }
            else {
                IconView.Visibility = ViewStates.Gone;
            }
        }

        void LoadIconImage(IImageSourceHandler handler, ImageSource source)
        {
            _iconTokenSource = new CancellationTokenSource();
            var token = _iconTokenSource.Token;
            Bitmap image = null;

            var scale = (float)_context.Resources.DisplayMetrics.Density;
            Task.Run(async () =>
            {
                image = await handler.LoadImageAsync(source, _context, token);
                token.ThrowIfCancellationRequested();
                image = CreateRoundImage(image);
            }, token).ContinueWith(t =>
            {
                if (t.IsCompleted) {
                    //entrust disposal of returned old image to Android OS.
                    ImageCacheController.Instance.Put(CellBase.IconSource.GetHashCode(), image);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Task.Delay(50); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
                        IconView.SetImageBitmap(image);
                        Invalidate();
                    });
                }
            });
        }

        Bitmap CreateRoundImage(Bitmap image)
        {
            var clipArea = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888);
            var canvas = new Canvas(clipArea);
            var paint = new Paint(PaintFlags.AntiAlias);
            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawRoundRect(new RectF(0, 0, image.Width, image.Height), _iconRadius, _iconRadius, paint);


            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

            var rect = new Rect(0, 0, image.Width, image.Height);
            canvas.DrawBitmap(image, rect, rect, paint);

            image.Recycle();
            image.Dispose();
            image = null;
            canvas.Dispose();
            canvas = null;
            paint.Dispose();
            paint = null;
            rect.Dispose();
            rect = null;

            return clipArea;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                CellBase.PropertyChanged -= CellPropertyChanged;
                CellParent.PropertyChanged -= ParentPropertyChanged;

                HintLabel?.Dispose();
                HintLabel = null;
                TitleLabel?.Dispose();
                TitleLabel = null;
                DescriptionLabel?.Dispose();
                DescriptionLabel = null;
                IconView?.SetImageDrawable(null);
                IconView?.SetImageBitmap(null);
                IconView?.Dispose();
                IconView = null;
                ContentStack?.Dispose();
                ContentStack = null;
                AccessoryStack?.Dispose();
                AccessoryStack = null;
                Cell = null;

                _iconTokenSource?.Dispose();
                _iconTokenSource = null;
                _context = null;

                _backgroundColor?.Dispose();
                _backgroundColor = null;
                _selectedColor?.Dispose();
                _selectedColor = null;

                Background?.Dispose();
                Background = null;
            }
            base.Dispose(disposing);
        }



    }

}
