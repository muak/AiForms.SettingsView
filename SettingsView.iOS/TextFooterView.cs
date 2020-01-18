using System;
using UIKit;
using System.Collections.Generic;

namespace AiForms.Renderers.iOS
{
    public class TextFooterView : UITableViewHeaderFooterView
    {
        public UILabel Label { get; set; }

        List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();
        UIEdgeInsets _curPadding;
        bool _isInitialized;

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
            if(_isInitialized && _curPadding == padding)
            {
                return;
            }

            foreach (var c in _constraints)
            {
                c.Active = false;
                c.Dispose();
            }
            _constraints.Clear();

            _constraints.Add(Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top));
            _constraints.Add(Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left));
            _constraints.Add(Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right));
            _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom));

            _constraints.ForEach(c => {
                c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
                c.Active = true;
            });

            _curPadding = padding;
            _isInitialized = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _constraints.ForEach(c => c.Dispose());
                Label?.Dispose();
                Label = null;
                BackgroundView?.Dispose();
                BackgroundView = null;
            }
        }
    }
}
