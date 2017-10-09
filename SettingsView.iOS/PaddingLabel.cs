using System;
using UIKit;
using CoreGraphics;
namespace AiForms.Renderers.iOS
{
    internal class PaddingLabel:UILabel
    {
        UIEdgeInsets _padding;

        public PaddingLabel()
        {
        }

        public override void DrawText(CoreGraphics.CGRect rect)
        {
            _padding = new UIEdgeInsets(8,14,4,14);
            var newRect = _padding.InsetRect(rect);
            base.DrawText(newRect);
        }

        public override CGSize IntrinsicContentSize {
            get {
                var contentSize =  base.IntrinsicContentSize;
                contentSize.Height += _padding.Top + _padding.Bottom;
                contentSize.Width += _padding.Left + _padding.Right;
                return contentSize;
            }
        }
    }
}
