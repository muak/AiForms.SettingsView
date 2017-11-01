using System;
using CoreGraphics;
using UIKit;

namespace AiForms.Renderers.iOS
{
    public class NoCaretField : UITextField
    {
        public NoCaretField() : base(new CGRect())
        {
        }

        public override CoreGraphics.CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }

    }
}
