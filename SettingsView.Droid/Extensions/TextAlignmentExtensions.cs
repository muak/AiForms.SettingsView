using System;
using Xamarin.Forms;
using GravityFlags = Android.Views.GravityFlags;

namespace AiForms.Renderers.Droid.Extensions
{
    public static class TextAlignmentExtensions
    {
        public static GravityFlags ToGravityFlags(this TextAlignment forms)
        {
            switch (forms) {
                case TextAlignment.Start:
                    return GravityFlags.Left | GravityFlags.CenterVertical;
                case TextAlignment.Center:
                    return GravityFlags.Center | GravityFlags.CenterVertical;
                case TextAlignment.End:
                    return GravityFlags.Right | GravityFlags.CenterVertical;
                default:
                    return GravityFlags.Right | GravityFlags.CenterVertical;
            }
        }
    }
}
