using System;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Renderers.iOS.Extensions
{
    /// <summary>
    /// Stack view alignment extensions.
    /// </summary>
    public static class StackViewAlignmentExtensions
    {
        /// <summary>
        /// Tos the native vertical.
        /// </summary>
        /// <returns>The native vertical.</returns>
        /// <param name="forms">Forms.</param>
        public static UIStackViewAlignment ToNativeVertical(this LayoutAlignment forms)
        {
            switch (forms) {
                case LayoutAlignment.Start:
                    return UIStackViewAlignment.Leading;
                case LayoutAlignment.Center:
                    return UIStackViewAlignment.Center;
                case LayoutAlignment.End:
                    return UIStackViewAlignment.Trailing;
                default:
                    return UIStackViewAlignment.Fill;
            }
        }

        /// <summary>
        /// Tos the native horizontal.
        /// </summary>
        /// <returns>The native horizontal.</returns>
        /// <param name="forms">Forms.</param>
        public static UIStackViewAlignment ToNativeHorizontal(this LayoutAlignment forms)
        {
            switch (forms) {
                case LayoutAlignment.Start:
                    return UIStackViewAlignment.Top;
                case LayoutAlignment.Center:
                    return UIStackViewAlignment.Center;
                case LayoutAlignment.End:
                    return UIStackViewAlignment.Bottom;
                default:
                    return UIStackViewAlignment.Fill;
            }
        }
    }
}
