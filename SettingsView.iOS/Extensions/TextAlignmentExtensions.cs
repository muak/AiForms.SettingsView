using System;
using UIKit;
using Xamarin.Forms;
namespace AiForms.Renderers.iOS.Extensions
{
    public static class TextAlignmentExtensions
    {
        public static UITextAlignment ToUITextAlignment(this TextAlignment forms)
        {
            switch (forms) {
                case TextAlignment.Start:
                    return UITextAlignment.Left;
                case TextAlignment.Center:
                    return UITextAlignment.Center;
                case TextAlignment.End:
                    return UITextAlignment.Right;
                default:
                    return UITextAlignment.Right;
            }
        }
    }
}
