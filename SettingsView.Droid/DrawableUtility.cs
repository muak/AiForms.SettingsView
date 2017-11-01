using System;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid
{
    public static class DrawableUtility
    {
        public static RippleDrawable CreateRipple(Android.Graphics.Color color, Drawable background = null)
        {
            if (background == null) {
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
