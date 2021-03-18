using System;
using CoreGraphics;
using UIKit;

namespace AiForms.Renderers.iOS
{
    public class PaddingLabel : UILabel
    {
        UIEdgeInsets _padding = new UIEdgeInsets();
        public UIEdgeInsets Padding {
            get => _padding;
            set {
                _padding = value;
                SetNeedsLayout();
                SetNeedsDisplay();
            }
        }

        public override void DrawText(CGRect rect)
        {
            base.DrawText(Padding.InsetRect(rect));
        }

        public override CGSize IntrinsicContentSize {
            get {
                var contentSize = base.IntrinsicContentSize;
                contentSize.Height += Padding.Top + Padding.Bottom;
                contentSize.Width += Padding.Left + Padding.Right;
                return contentSize;
            }
        }
    }
}
