using System;
using UIKit;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AiForms.Renderers.iOS
{
    public class TextFooterView : UITableViewHeaderFooterView
    {
        public PaddingLabel Label { get; set; }
        List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();

        public TextFooterView(IntPtr handle):base(handle)
        {
            Label = new PaddingLabel();
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.CharacterWrap;
            Label.TranslatesAutoresizingMaskIntoConstraints = false;

            ContentView.AddSubview(Label);

            _constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0));
            _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0));
            _constraints.Add(Label.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, 0));
            _constraints.Add(Label.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, 0));

            _constraints.ForEach(c => {
                c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
                c.Active = true;
            });

            this.BackgroundView = new UIView();
        }

        public void UpdateTextAlignment(Section section)
        {
	        if ( section is null ) return;
	        UpdateTextAlignment(section.TextAlignment);
        }
        public void UpdateTextAlignment(TextAlignment alignment)
        {
	        Label.TextAlignment = CellBaseView.GetTextAlignment(alignment);
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
