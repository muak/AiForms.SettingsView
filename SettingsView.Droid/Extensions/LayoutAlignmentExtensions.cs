using System;
using Android.Views;
using Xamarin.Forms;
namespace AiForms.Renderers.Droid.Extensions
{
    public static class LayoutAlignmentExtensions
    {
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
