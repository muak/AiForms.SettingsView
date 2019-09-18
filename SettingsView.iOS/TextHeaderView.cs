using System;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Renderers.iOS
{
    public class TextHeaderView : UITableViewHeaderFooterView
    {
        public UILabel Label { get; set; }
        public bool IsInitialized { get; private set; } = false;

        public TextHeaderView(IntPtr handle): base(handle) 
        {
            Label = new UILabel();
            Label.Lines = 1;
            Label.LineBreakMode = UILineBreakMode.TailTruncation;
            Label.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddSubview(Label);

            this.BackgroundView = new UIView();
        }
        public void Initialzie(UIEdgeInsets padding, LayoutAlignment align)
        {
            Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left).Active = true;
            Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right).Active = true;

            if (align == LayoutAlignment.Start)
            {
                Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top).Active = true;
            }
            else if (align == LayoutAlignment.End)
            {
                Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom).Active = true;
            }
            else
            {
                Label.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor, 0).Active = true;
            }

            IsInitialized = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing)
            {
                Label?.Dispose();
                Label = null;
                BackgroundView?.Dispose();
                BackgroundView = null;
            }
        }
    }
}
