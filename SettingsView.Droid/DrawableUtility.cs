using System;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid
{
    public static class DrawableUtility
    {
        public static StateListDrawable CreateSelector(Android.Graphics.Color color, Drawable background = null)
        {
            var focusDrawable = new GradientDrawable();
            focusDrawable.SetShape(ShapeType.Rectangle);
            focusDrawable.SetColor(color);

            var pressDrawable = new GradientDrawable();
            pressDrawable.SetShape(ShapeType.Rectangle);
            pressDrawable.SetColor(Android.Graphics.Color.Transparent);

            var ripple = CreateRipple(color,background);

            var sel = new StateListDrawable();
            sel.AddState(new int[] { global::Android.Resource.Attribute.StateSelected }, focusDrawable);
            sel.AddState(new int[] { -global::Android.Resource.Attribute.StateSelected }, pressDrawable);
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
