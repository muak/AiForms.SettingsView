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
    /// <summary>
    /// Label cell renderer.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

    /// <summary>
    /// Label cell view.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class LabelCellView : CellBaseView
    {
        /// <summary>
        /// Gets or sets the value label.
        /// </summary>
        /// <value>The value label.</value>
        public UILabel ValueLabel { get; set; }
        LabelCell _LabelCell => Cell as LabelCell;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.LabelCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public LabelCellView(Cell formsCell) : base(formsCell)
        {
            ValueLabel = new UILabel();
            ValueLabel.TextAlignment = UITextAlignment.Right;

            ContentStack.AddArrangedSubview(ValueLabel);
            ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
            ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
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

        /// <summary>
        /// Parents the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
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

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            UpdateValueText();
            UpdateValueTextColor();
            UpdateValueTextFontSize();
            base.UpdateCell();
        }

        /// <summary>
        /// Sets the enabled appearance.
        /// </summary>
        /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
        protected override void SetEnabledAppearance(bool isEnabled)
        {
            if(isEnabled){
                ValueLabel.Alpha = 1f;
            }
            else{
                ValueLabel.Alpha = 0.3f;
            }
            base.SetEnabledAppearance(isEnabled);
        }

        /// <summary>
        /// Updates the value text.
        /// </summary>
        protected void UpdateValueText()
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

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
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
