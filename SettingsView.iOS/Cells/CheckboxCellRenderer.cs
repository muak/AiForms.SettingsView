using System;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class CheckboxCellRenderer:CellBaseRenderer<CheckboxCellView>{}
   
    public class CheckboxCellView:CellBaseView
    {
        CheckBox _checkbox;
        CheckboxCell _CheckboxCell => Cell as CheckboxCell;

        public CheckboxCellView(Cell formsCell):base(formsCell)
        {
            _checkbox = new CheckBox(new CGRect(0, 0, 20, 20));
            _checkbox.Layer.BorderWidth = 2;
            _checkbox.Layer.CornerRadius = 3;
            _checkbox.Inset = new UIEdgeInsets(10, 10, 10, 10);

            _checkbox.CheckChanged = CheckChanged;

            AccessoryView = _checkbox;
        }

        public override void UpdateCell()
        {
            UpdateAccentColor();
            UpdateChecked();
            base.UpdateCell();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName)
            {
                UpdateAccentColor();
            }
            if (e.PropertyName == CheckboxCell.CheckedProperty.PropertyName)
            {
                UpdateChecked();
            }
        }

        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
            {
                UpdateAccentColor();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _checkbox.CheckChanged = null;
                _checkbox?.Dispose();
                _checkbox = null;
            }
            base.Dispose(disposing);
        }

        void CheckChanged(UIButton button)
        {
            _CheckboxCell.Checked = button.Selected;
        }

        void UpdateChecked()
        {
            _checkbox.Selected = _CheckboxCell.Checked;
        }

        void UpdateAccentColor()
        {
            if (_CheckboxCell.AccentColor != Xamarin.Forms.Color.Default)
            {
                ChangeCheckColor(_CheckboxCell.AccentColor.ToCGColor());
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default)
            {
                ChangeCheckColor(CellParent.CellAccentColor.ToCGColor());
            }
        }

        void ChangeCheckColor(CGColor accent){
            _checkbox.Layer.BorderColor = accent;
            _checkbox.FillColor = accent;
            _checkbox.SetNeedsDisplay(); //update inner rect
        }
    }

    public class CheckBox : UIButton
    {
        public UIEdgeInsets Inset { get; set; } = new UIEdgeInsets(20, 20, 20, 20);
        public CGColor FillColor { get; set; }
        public Action<UIButton> CheckChanged { get; set; }

        public CheckBox(CGRect rect) : base(rect)
        {
            this.AddGestureRecognizer(new UITapGestureRecognizer((obj) => {
                Selected = !Selected;
                CheckChanged?.Invoke(this);
            }));
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var lineWidth = rect.Size.Width / 10;

            // Draw check mark
            if (Selected)
            {
                this.Layer.BackgroundColor = FillColor;


                var checkmark = new UIBezierPath();
                var size = rect.Size;
                checkmark.MoveTo(new CGPoint(x: 22f / 100f * size.Width, y: 52f / 100f * size.Height));
                checkmark.AddLineTo(new CGPoint(x: 38f / 100f * size.Width, y: 68f / 100f * size.Height));
                checkmark.AddLineTo(new CGPoint(x: 76f / 100f * size.Width, y: 30f / 100f * size.Height));

                checkmark.LineWidth = lineWidth;
                UIColor.White.SetStroke();
                checkmark.Stroke();
            }

            else
            {
                this.Layer.BackgroundColor = new CGColor(0, 0, 0, 0);
            }
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var rect = this.Bounds;
            rect.X -= Inset.Left;
            rect.Y -= Inset.Top;
            rect.Width += Inset.Left + Inset.Right;
            rect.Height += Inset.Top + Inset.Bottom;

            return rect.Contains(point);
        }

    }
}
