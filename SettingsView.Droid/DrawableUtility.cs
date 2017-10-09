using System;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid
{
    public static class DrawableUtility
    {
        public static StateListDrawable CreateSelector()
        {
            var focusDrawable = new GradientDrawable();
            focusDrawable.SetShape(ShapeType.Rectangle);
            focusDrawable.SetColor(Android.Graphics.Color.Green);

            var pressDrawable = new GradientDrawable();
            pressDrawable.SetShape(ShapeType.Rectangle);
            pressDrawable.SetColor(Android.Graphics.Color.Red);

            var ripple = CreateRipple(Android.Graphics.Color.Red);

            var sel = new StateListDrawable();
            sel.AddState(new int[] { global::Android.Resource.Attribute.StateSelected }, focusDrawable);
            sel.AddState(new int[] { global::Android.Resource.Attribute.StatePressed }, ripple);

            return sel;
        }
        public static RippleDrawable CreateRipple(Android.Graphics.Color color,Drawable background = null)
        {
            if (background == null)
            {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return new RippleDrawable(getPressedColorSelector(color), null, mask);
            }

            return new RippleDrawable(getPressedColorSelector(color), background, null);

            //var back = _view.Background;
            //if (back == null)
            //{
            //    var mask = new ColorDrawable(Android.Graphics.Color.White);
            //    return _ripple = new RippleDrawable(getPressedColorSelector(color), null, mask);
            //}
            //else if (back is RippleDrawable)
            //{
            //    _ripple = back.GetConstantState().NewDrawable() as RippleDrawable;
            //    _ripple.SetColor(getPressedColorSelector(color));

            //    return _ripple;
            //}
            //else
            //{
            //    return _ripple = new RippleDrawable(getPressedColorSelector(color), back, null);
            //}
        }

        public static ColorStateList getPressedColorSelector(int pressedColor)
        {
            return new ColorStateList(
                new int[][]
                {
                    new int[]{}
                },
                new int[]
                {
                    pressedColor,
                });
        }
    }
}
