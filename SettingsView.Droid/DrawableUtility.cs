using System;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Drawable utility.
    /// </summary>
    public static class DrawableUtility
    {
        /// <summary>
        /// Creates the ripple.
        /// </summary>
        /// <returns>The ripple.</returns>
        /// <param name="color">Color.</param>
        /// <param name="background">Background.</param>
        public static RippleDrawable CreateRipple(Android.Graphics.Color color, Drawable background = null)
        {
            if (background == null) {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return new RippleDrawable(getPressedColorSelector(color), null, mask);
            }

            return new RippleDrawable(getPressedColorSelector(color), background, null);
        }

        /// <summary>
        /// Gets the pressed color selector.
        /// </summary>
        /// <returns>The pressed color selector.</returns>
        /// <param name="pressedColor">Pressed color.</param>
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
