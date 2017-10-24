using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;

namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Refer to https://qiita.com/Hoshi_7/items/e950203e615aebe3569d
    /// Refer to http://y-anz-m.blogspot.jp/2014/03/androidimageview.html
    /// </summary>
    [Preserve(AllMembers = true)]
    [Register("aiforms.renderers.droid.RoundImageView")]
    public class RoundImageView:ImageView
    {
        Bitmap _image;
        //public float CornerRadius
        //{
        //    get { return mMaskDrawable.CornerRadius; }
        //    set { mMaskDrawable.SetCornerRadius(value);}
        //}
        public float CornerRadius { get; set; } = 18f;

        Paint mMaskedPaint;
        Paint mCopyPaint;
        GradientDrawable mMaskDrawable;

        public RoundImageView(Context context):this(context,null){}
        public RoundImageView(Context context,IAttributeSet attrs):base(context,attrs)
        {
            
        }

        public override void SetImageDrawable(Drawable drawable)
        {
            var image = (drawable as BitmapDrawable)?.Bitmap;
            SetRoundBitmap(image);
        }

        public override void SetImageResource(int resId)
        {
            var image = BitmapFactory.DecodeResource(Context.Resources, resId);
            SetRoundBitmap(image);
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            SetRoundBitmap(bm);
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            base.SetImageDrawable(null);
            Background = null;

            DestroyDrawingCache();

            _image?.Recycle();
            _image?.Dispose();
            _image = null;
        }

        void SetRoundBitmap(Bitmap image)
        {
            if (_image != null) {
                _image.Recycle();
                _image.Dispose();
                _image = null;
            }

            if(image == null){
                //base.SetImageDrawable(null);
                //base.SetImageBitmap(null);
                return;
            }

            int width = image.Width;
            int height = image.Height;

            Bitmap clipArea = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            Canvas c = new Canvas(clipArea);
            c.DrawARGB(0, 0, 0, 0);
            c.DrawRoundRect(new RectF(0, 0, width, height), CornerRadius, CornerRadius, new Paint(PaintFlags.AntiAlias));



            _image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(_image);
            Paint paint = new Paint();
            canvas.DrawBitmap(clipArea, 0, 0, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(image, new Rect(0, 0, width, height), new Rect(0, 0, width, height), paint);
            clipArea.Recycle();
            clipArea.Dispose();
            clipArea = null;
            base.SetImageDrawable(new BitmapDrawable(Context.Resources, _image));

        }


        //public RoundImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        //{
        //    mMaskedPaint = new Paint();
        //    mMaskedPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcAtop));

        //    mCopyPaint = new Paint();
        //    var shape = new GradientDrawable();
        //    shape.SetShape(ShapeType.Rectangle);
        //    shape.SetColor(Android.Graphics.Color.Argb(1,255,255,255));
        //    shape.SetCornerRadius(18f);
        //    mMaskDrawable = shape;
        //}

        //Rect mBounds;
        //RectF mBoundsF;

        //protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        //{
        //    mBounds = new Rect(0, 0, w, h);
        //    mBoundsF = new RectF(mBounds);
        //}

        //protected override void OnDraw(Canvas canvas)
        //{
        //    int sc = canvas.SaveLayer(mBoundsF, mCopyPaint, SaveFlags.HasAlphaLayer | SaveFlags.FullColorLayer);

        //    mMaskDrawable.SetBounds(mBounds.Left,mBounds.Top,mBounds.Right,mBounds.Bottom);
        //    mMaskDrawable.Draw(canvas);

        //    canvas.SaveLayer(mBoundsF,mMaskedPaint,0);

        //    base.OnDraw(canvas);

        //    canvas.RestoreToCount(sc);
        //}
    }
}
