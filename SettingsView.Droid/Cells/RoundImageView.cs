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
    /// Refer to http://y-anz-m.blogspot.jp/2014/03/androidimageview.html
    /// </summary>
    [Preserve(AllMembers = true)]
    [Register("aiforms.renderers.droid.RoundImageView")]
    public class RoundImageView:ImageView
    {
        public float CornerRadius
        {
            get { return mMaskDrawable.CornerRadius; }
            set { mMaskDrawable.SetCornerRadius(value);}
        }

        Paint mMaskedPaint;
        Paint mCopyPaint;
        GradientDrawable mMaskDrawable;

        public RoundImageView(Context context):this(context,null){}
        public RoundImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            mMaskedPaint = new Paint();
            mMaskedPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcAtop));

            mCopyPaint = new Paint();
            var shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetColor(Android.Graphics.Color.White);
            shape.SetCornerRadius(18f);
            mMaskDrawable = shape;
        }

        Rect mBounds;
        RectF mBoundsF;

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            mBounds = new Rect(0, 0, w, h);
            mBoundsF = new RectF(mBounds);
        }

        protected override void OnDraw(Canvas canvas)
        {
            int sc = canvas.SaveLayer(mBoundsF, mCopyPaint, SaveFlags.HasAlphaLayer | SaveFlags.FullColorLayer);

            mMaskDrawable.SetBounds(mBounds.Left,mBounds.Top,mBounds.Right,mBounds.Bottom);
            mMaskDrawable.Draw(canvas);

            canvas.SaveLayer(mBoundsF,mMaskedPaint,0);

            base.OnDraw(canvas);

            canvas.RestoreToCount(sc);
        }
    }
}
