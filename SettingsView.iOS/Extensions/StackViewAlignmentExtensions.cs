using System;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Renderers.iOS.Extensions
{
    public static class StackViewAlignmentExtensions
    {
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
