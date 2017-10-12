using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;
using Android.Graphics.Drawables;
using Android.Content.Res;

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
        public RoundImageView IconView { get; set; }
        public LinearLayout ContentStack { get; set; }
        public LinearLayout AccessoryStack { get; set; }
        public TextView HintLabel { get; private set; }

        Context _context;
        CancellationTokenSource _iconTokenSource;
        Android.Graphics.Color _defaultTextColor;
        ColorDrawable _backgroundColor;
        float _defaultFontSize;

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

            IconView = contentView.FindViewById<RoundImageView>(Resource.Id.CellIcon);
            TitleLabel = contentView.FindViewById<TextView>(Resource.Id.CellTitle);
            DescriptionLabel = contentView.FindViewById<TextView>(Resource.Id.CellDescription);
            ContentStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellContentStack);
            AccessoryStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryView);
            HintLabel = contentView.FindViewById<TextView>(Resource.Id.CellHintText);

            _backgroundColor = new ColorDrawable();
            var sel = new StateListDrawable();

            sel.AddState(new int[] { global::Android.Resource.Attribute.StateSelected },
                         new ColorDrawable(Android.Graphics.Color.Argb(125,180, 180, 180)));
            sel.AddState(new int[]{-global::Android.Resource.Attribute.StateSelected},_backgroundColor);
            Background = sel;

            _defaultTextColor = new Android.Graphics.Color(TitleLabel.CurrentTextColor);
            _defaultFontSize = TitleLabel.TextSize;
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CellBase.TitleProperty.PropertyName)
            {
                UpdateTitleText();
            }
            else if (e.PropertyName == CellBase.TitleColorProperty.PropertyName)
            {
                UpdateTitleColor();
            }
            else if (e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName)
            {
                UpdateTitleFontSize();
            }
            else if (e.PropertyName == CellBase.DescriptionProperty.PropertyName)
            {
                UpdateDescriptionText();
            }
            else if (e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName)
            {
                UpdateDescriptionFontSize();
            }
            else if (e.PropertyName == CellBase.DescriptionColorProperty.PropertyName)
            {
                UpdateDescriptionFontSize();
            }
            else if (e.PropertyName == CellBase.IconSourceProperty.PropertyName)
            {
                UpdateIcon();
            }
            else if (e.PropertyName == CellBase.BackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == CellBase.HintTextProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateHintText);
            }
            else if (e.PropertyName == CellBase.HintTextColorProperty.PropertyName)
            {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == CellBase.HintFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if (e.PropertyName == CellBase.IconSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateIconSize);
            }
            else if(e.PropertyName == CellBase.IconRadiusProperty.PropertyName){
                UpdateWithForceLayout(UpdateIconRadius);
            }
        }

        public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName)
            {
                UpdateTitleColor();
            }
            else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName)
            {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == SettingsView.CellHintTextColorProperty.PropertyName)
            {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if(e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName){
               UpdateWithForceLayout(UpdateIcon);
            }
            else if( e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName){
                UpdateIconRadius();
                UpdateWithForceLayout(UpdateIcon);
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

            if (CellBase.BackgroundColor != Xamarin.Forms.Color.Default)
            {
                _backgroundColor.Color = CellBase.BackgroundColor.ToAndroid();
                //SetBackgroundColor(CellBase.BackgroundColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellBackgroundColor != Xamarin.Forms.Color.Default)
            {
                _backgroundColor.Color = CellParent.CellBackgroundColor.ToAndroid();
                //SetBackgroundColor(CellParent.CellBackgroundColor.ToAndroid());
            }
            else{
                _backgroundColor.Color = Android.Graphics.Color.Transparent;
            }
        }

        void UpdateTitleText()
        {
            TitleLabel.Text = CellBase.Title;
        }

        void UpdateTitleColor()
        {
            if (CellBase.TitleColor != Xamarin.Forms.Color.Default)
            {
                TitleLabel.SetTextColor(CellBase.TitleColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellTitleColor != Xamarin.Forms.Color.Default)
            {
                TitleLabel.SetTextColor(CellParent.CellTitleColor.ToAndroid());
            }
            else{
                TitleLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateTitleFontSize()
        {
            if (CellBase.TitleFontSize > 0)
            {
                TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.TitleFontSize);
            }
            else if (CellParent != null)
            {
                TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellTitleFontSize);
            }
            else{
                TitleLabel.SetTextSize(ComplexUnitType.Sp,_defaultFontSize);
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
            if (CellBase.DescriptionFontSize > 0)
            {
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.DescriptionFontSize);
            }
            else if (CellParent != null)
            {
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellDescriptionFontSize);
            }
            else{
                DescriptionLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
            }
        }

        void UpdateDescriptionColor()
        {
            if (CellBase.DescriptionColor != Xamarin.Forms.Color.Default)
            {
                DescriptionLabel.SetTextColor(CellBase.DescriptionColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellDescriptionColor != Xamarin.Forms.Color.Default)
            {
                DescriptionLabel.SetTextColor(CellParent.CellDescriptionColor.ToAndroid());
            }
            else{
                DescriptionLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateHintText()
        {
            var msg = CellBase.HintText;
            if (string.IsNullOrEmpty(msg))
            {
                HintLabel.Visibility = ViewStates.Gone;
                return;
            }

            HintLabel.Text = msg;
            HintLabel.Visibility = ViewStates.Visible;
        }

        void UpdateHintTextColor()
        {
            if (CellBase.HintTextColor != Xamarin.Forms.Color.Default)
            {
                HintLabel.SetTextColor(CellBase.HintTextColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellHintTextColor != Xamarin.Forms.Color.Default)
            {
                HintLabel.SetTextColor(CellParent.CellHintTextColor.ToAndroid());
            }
            else
            {
                HintLabel.SetTextColor(_defaultTextColor);
            }
        }

        void UpdateHintFontSize()
        {
            if (CellBase.HintFontSize > 0)
            {
                HintLabel.SetTextSize(ComplexUnitType.Sp, (float)CellBase.HintFontSize);
            }
            else if (CellParent != null)
            {
                HintLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellHintFontSize);
            }
            else
            {
                HintLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
            }
        }

        void UpdateIconRadius()
        {
            if (CellBase.IconRadius >= 0)
            {
                IconView.CornerRadius = _context.ToPixels(CellBase.IconRadius);
            }
            else if (CellParent != null)
            {
                IconView.CornerRadius = _context.ToPixels(CellParent.CellIconRadius);
            }
        }

        void UpdateIconSize()
        {
            Xamarin.Forms.Size size;
            if (CellBase.IconSize != default(Xamarin.Forms.Size))
            {
                size = CellBase.IconSize;
            }
            else if (CellParent != null && CellParent.CellIconSize != default(Xamarin.Forms.Size))
            {
                size = CellParent.CellIconSize;
            }
            else
            {
                size = new Xamarin.Forms.Size(36, 36);
            }

            IconView.LayoutParameters.Width = (int)_context.ToPixels(size.Width);
            IconView.LayoutParameters.Height = (int)_context.ToPixels(size.Height);
            //IconView.CornerRadius = _context.ToPixels(12);
        }

        void UpdateIcon()
        {

            if (_iconTokenSource != null && !_iconTokenSource.IsCancellationRequested)
            {
                //前のがキャンセルされてなければとりあえずキャンセル
                _iconTokenSource.Cancel();
            }

            UpdateIconSize();

            if (IconView.Drawable != null)
            {
                IconView.SetImageBitmap(null);
            }

            if (CellBase.IconSource != null)
            {
                IconView.Visibility = ViewStates.Visible;

                var cache = ImageCacheController.Instance.Get(CellBase.IconSource.GetHashCode()) as Bitmap;
                if (cache != null)
                {
                    IconView.SetImageBitmap(cache);
                    return;
                }

                var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(CellBase.IconSource.GetType());
                LoadIconImage(handler, CellBase.IconSource);
            }
            else
            {
                IconView.Visibility = ViewStates.Gone;
            }
        }

        void LoadIconImage(IImageSourceHandler handler, ImageSource source)
        {
            _iconTokenSource = new CancellationTokenSource();
            var token = _iconTokenSource.Token;
            Bitmap image = null;

            var scale = (float)_context.Resources.DisplayMetrics.Density;
            Task.Run(async () => {
                image = await handler.LoadImageAsync(source, _context, token);
                token.ThrowIfCancellationRequested();
            }, token).ContinueWith(t => {
                if (t.IsCompleted)
                {
                    ImageCacheController.Instance.Put(CellBase.IconSource.GetHashCode(), image);
                    Device.BeginInvokeOnMainThread(() => {
                        IconView.SetImageBitmap(image);
                        Invalidate();
                    });
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CellBase.PropertyChanged -= CellPropertyChanged;
                CellParent.PropertyChanged -= ParentPropertyChanged;

                HintLabel?.Dispose();
                HintLabel = null;
                TitleLabel?.Dispose();
                TitleLabel = null;
                DescriptionLabel?.Dispose();
                DescriptionLabel = null;
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
            }
            base.Dispose(disposing);
        }



    }

}
