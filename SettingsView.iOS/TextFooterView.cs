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
        NSLayoutConstraint _leftConstraint;
        UITableView _tableView;

        public TextFooterView(IntPtr handle):base(handle)
        {
            Label = new UILabel();
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.WordWrap;
            Label.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddSubview(Label);

            this.BackgroundView = new UIView();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_leftConstraint != null)
            {
                _leftConstraint.Active = false;
                _leftConstraint.Dispose();
                _leftConstraint = null;
            }

            if (Label is null)
                return; // For HotReload

            _leftConstraint = Label.LeftAnchor.ConstraintEqualTo(LeftAnchor, _curPadding.Left + _tableView.SafeAreaInsets.Left);
            _leftConstraint.Active = true;
        }

        public void Initialzie(UIEdgeInsets padding, UITableView tableView)
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
            _constraints.Add(Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right));
            _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom));

            _constraints.ForEach(c => {
                c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
                c.Active = true;
            });

            _curPadding = padding;
            _tableView = tableView;
            _isInitialized = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _constraints.ForEach(c => c.Dispose());
                _leftConstraint?.Dispose();
                _leftConstraint = null;
                Label?.Dispose();
                Label = null;
                BackgroundView?.Dispose();
                BackgroundView = null;
                _tableView = null;
            }
        }
    }
}
