using System;
using CoreGraphics;
using UIKit;

namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// No caret field.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class NoCaretField : UITextField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.NoCaretField"/> class.
        /// </summary>
        public NoCaretField() : base(new CGRect())
        {
        }
        /// <summary>
        /// Gets the caret rect for position.
        /// </summary>
        /// <returns>The caret rect for position.</returns>
        /// <param name="position">Position.</param>
        public override CoreGraphics.CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }

    }
}
