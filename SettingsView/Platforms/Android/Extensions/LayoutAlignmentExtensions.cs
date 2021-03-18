using System;
using Android.Views;
using Xamarin.Forms;
namespace AiForms.Renderers.Droid.Extensions
{
    /// <summary>
    /// Layout alignment extensions.
    /// </summary>
    [Android.Runtime.Preserve (AllMembers = true)]
    public static class LayoutAlignmentExtensions
    {
        /// <summary>
        /// Tos the native vertical.
        /// </summary>
        /// <returns>The native vertical.</returns>
        /// <param name="forms">Forms.</param>
        public static GravityFlags ToNativeVertical(this LayoutAlignment forms)
        {
            switch (forms) {
                case LayoutAlignment.Start:
                    return GravityFlags.Top;
                case LayoutAlignment.Center:
                    return GravityFlags.CenterVertical;
                case LayoutAlignment.End:
                    return GravityFlags.Bottom;
                default:
                    return GravityFlags.FillHorizontal;
            }
        }

        /// <summary>
        /// Tos the native horizontal.
        /// </summary>
        /// <returns>The native horizontal.</returns>
        /// <param name="forms">Forms.</param>
        public static GravityFlags ToNativeHorizontal(this LayoutAlignment forms)
        {
            switch (forms) {
                case LayoutAlignment.Start:
                    return GravityFlags.Start;
                case LayoutAlignment.Center:
                    return GravityFlags.CenterHorizontal;
                case LayoutAlignment.End:
                    return GravityFlags.End;
                default:
                    return GravityFlags.FillVertical;
            }
        }
    }
}
