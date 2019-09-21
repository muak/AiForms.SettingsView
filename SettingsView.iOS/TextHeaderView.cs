using System;
using UIKit;
using Xamarin.Forms;
using System.Collections.Generic;

namespace AiForms.Renderers.iOS
{
    public class TextHeaderView : UITableViewHeaderFooterView
    {
        public UILabel Label { get; set; }
        List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();
        UIEdgeInsets _curPadding;
        LayoutAlignment _curAlignment;
        bool _isInitialized;

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
            if(_isInitialized && _curPadding == padding && align == _curAlignment)
            {
                return;
            }

            foreach (var c in _constraints)
            {
                c.Active = false;
                c.Dispose();
            }
            _constraints.Clear();

            _constraints.Add(Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left));
            _constraints.Add(Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right));

            if (align == LayoutAlignment.Start)
            {
                _constraints.Add(Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top));
            }
            else if (align == LayoutAlignment.End)
            {
                _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom));
            }
            else
            {
                _constraints.Add(Label.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor, 0));
            }

            _constraints.ForEach(c => {
                c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
                c.Active = true;
            });

            _curPadding = padding;
            _curAlignment = align;
            _isInitialized = true;         
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing)
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
