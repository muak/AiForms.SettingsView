using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

    public class LabelCellView : CellBaseView
    {
        public UILabel ValueLabel { get; set; }
        LabelCell _LabelCell => Cell as LabelCell;

        public LabelCellView(Cell formsCell) : base(formsCell)
        {
            ValueLabel = new UILabel();
            ValueLabel.TextAlignment = UITextAlignment.Right;

            ContentStack.AddArrangedSubview(ValueLabel);
            ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
            ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
        }

        public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);

            if (e.PropertyName == LabelCell.ValueTextProperty.PropertyName) {
                UpdateValueText();
            }
            else if (e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateValueTextFontSize);
            }
            else if (e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName) {
                UpdateValueTextColor();
            }
        }

        public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);

            if (e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName) {
                UpdateValueTextColor();
            }
            else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateValueTextFontSize);
            }
        }

        public override void UpdateCell()
        {
            UpdateValueText();
            UpdateValueTextColor();
            UpdateValueTextFontSize();
            base.UpdateCell();
        }

        void UpdateValueText()
        {
            ValueLabel.Text = _LabelCell.ValueText;
        }

        void UpdateValueTextFontSize()
        {
            if (_LabelCell.ValueTextFontSize > 0) {
                ValueLabel.Font = ValueLabel.Font.WithSize((nfloat)_LabelCell.ValueTextFontSize);
            }
            else if (CellParent != null) {
                ValueLabel.Font = ValueLabel.Font.WithSize((nfloat)CellParent.CellValueTextFontSize);
            }
        }

        void UpdateValueTextColor()
        {
            if (_LabelCell.ValueTextColor != Xamarin.Forms.Color.Default) {
                ValueLabel.TextColor = _LabelCell.ValueTextColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default) {
                ValueLabel.TextColor = CellParent.CellValueTextColor.ToUIColor();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                ContentStack.RemoveArrangedSubview(ValueLabel);
                ValueLabel.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
