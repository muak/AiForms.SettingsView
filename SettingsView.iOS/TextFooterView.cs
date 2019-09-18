using System;
using UIKit;

namespace AiForms.Renderers.iOS
{
    public class TextFooterView : UITableViewHeaderFooterView
    {
        public UILabel Label { get; set; }
        public bool IsInitialized { get; private set; } = false;

        public TextFooterView(IntPtr handle):base(handle)
        {
            Label = new UILabel();
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.WordWrap;
            Label.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddSubview(Label);

            this.BackgroundView = new UIView();
        }

        public void Initialzie(UIEdgeInsets padding)
        {
            Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top).Active = true;
            Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left).Active = true;
            Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right).Active = true;
            Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom).Active = true;

            IsInitialized = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Label?.Dispose();
                Label = null;
                BackgroundView?.Dispose();
                BackgroundView = null;
            }
        }
    }
}
