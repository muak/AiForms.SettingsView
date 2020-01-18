using System;
using Xamarin.Forms;
using AiEntryCell = AiForms.Renderers.EntryCell;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using AiForms.Renderers.iOS.Extensions;
using Foundation;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(AiForms.Renderers.iOS.EntryCellRenderer))]
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Entry cell renderer.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

    /// <summary>
    /// Entry cell view.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class EntryCellView : CellBaseView
    {
        AiEntryCell _EntryCell => Cell as AiEntryCell;
        internal UITextField ValueField;
        UIView _FieldWrapper;
        bool _hasFocus = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.EntryCellView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public EntryCellView(Cell formsCell) : base(formsCell)
        {
            ValueField = new UITextField() { BorderStyle = UITextBorderStyle.None };
            ValueField.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            ValueField.EditingChanged += _textField_EditingChanged;
            ValueField.EditingDidBegin += ValueField_EditingDidBegin;
            ValueField.EditingDidEnd += ValueField_EditingDidEnd;
            ValueField.ShouldReturn = OnShouldReturn;

            _EntryCell.Focused += EntryCell_Focused;


            _FieldWrapper = new UIView();
            _FieldWrapper.AutosizesSubviews = true;
            _FieldWrapper.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
            _FieldWrapper.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);

            _FieldWrapper.AddSubview(ValueField);
            ContentStack.AddArrangedSubview(_FieldWrapper);
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateValueText();
            UpdateValueTextColor();
            UpdateValueTextFontSize();
            UpdatePlaceholder();
            UpdateKeyboard();
            UpdateIsPassword();
            UpdateTextAlignment();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == AiEntryCell.ValueTextProperty.PropertyName) {
                UpdateValueText();
            }
            else if (e.PropertyName == AiEntryCell.ValueTextFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateValueTextFontSize);
            }
            else if (e.PropertyName == AiEntryCell.ValueTextColorProperty.PropertyName) {
                UpdateValueTextColor();
            }
            else if (e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName) {
                UpdateKeyboard();
            }
            else if (e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName) {
                UpdatePlaceholder();
            }
            else if (e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName) {
                UpdateTextAlignment();
            }
            else if (e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName) {
                UpdateIsPassword();
            }
        }

        /// <summary>
        /// Parents the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName) {
                UpdateValueTextColor();
                ValueField.SetNeedsLayout();    // immediately reflect
            }
            else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateValueTextFontSize);
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ValueField.BecomeFirstResponder();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                ValueField.EditingChanged -= _textField_EditingChanged;
                ValueField.EditingDidBegin -= ValueField_EditingDidBegin;
                ValueField.EditingDidEnd -= ValueField_EditingDidEnd;
                _EntryCell.Focused -= EntryCell_Focused;
                ValueField.ShouldReturn = null;
                ValueField.RemoveFromSuperview();
                ValueField.Dispose();
                ValueField = null;

                ContentStack.RemoveArrangedSubview(_FieldWrapper);
                _FieldWrapper.Dispose();
                _FieldWrapper = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets the enabled appearance.
        /// </summary>
        /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
        protected override void SetEnabledAppearance(bool isEnabled)
        {
            if(isEnabled){
                ValueField.Alpha = 1.0f;
            }
            else{
                ValueField.Alpha = 0.3f;
            }
            base.SetEnabledAppearance(isEnabled);
        }

        void UpdateValueText()
        {
            //Without this judging, TextField don't correctly work when inputting Japanese (maybe other 2byte languages either).
            if (ValueField.Text != _EntryCell.ValueText)
            {
                ValueField.Text = _EntryCell.ValueText;
            }
        }

        void UpdateValueTextFontSize()
        {
            if (_EntryCell.ValueTextFontSize > 0) {
                ValueField.Font = ValueField.Font.WithSize((nfloat)_EntryCell.ValueTextFontSize);
            }
            else if (CellParent != null) {
                ValueField.Font = ValueField.Font.WithSize((nfloat)CellParent.CellValueTextFontSize);
            }
            //make the view height fit font size
            var contentH = ValueField.IntrinsicContentSize.Height;
            var bounds = ValueField.Bounds;
            ValueField.Bounds = new CoreGraphics.CGRect(0, 0, bounds.Width, contentH);
            _FieldWrapper.Bounds = new CoreGraphics.CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
        }

        void UpdateValueTextColor()
        {
            if (_EntryCell.ValueTextColor != Xamarin.Forms.Color.Default) {
                ValueField.TextColor = _EntryCell.ValueTextColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default) {
                ValueField.TextColor = CellParent.CellValueTextColor.ToUIColor();
            }
            ValueField.SetNeedsLayout();
        }

        void UpdateKeyboard()
        {
            ValueField.ApplyKeyboard(_EntryCell.Keyboard);
        }

        void UpdatePlaceholder()
        {
            ValueField.Placeholder = _EntryCell.Placeholder;
        }

        void UpdateTextAlignment()
        {
            ValueField.TextAlignment = _EntryCell.TextAlignment.ToUITextAlignment();
            ValueField.SetNeedsLayout();
        }

        void UpdateIsPassword()
        {
            ValueField.SecureTextEntry = _EntryCell.IsPassword;
        }


        void _textField_EditingChanged(object sender, EventArgs e)
        {
            _EntryCell.ValueText = ValueField.Text;
        }


        void ValueField_EditingDidBegin(object sender, EventArgs e)
        {
            _hasFocus = true;
        }


        void ValueField_EditingDidEnd(object sender, EventArgs e)
        {           
            if(!_hasFocus)
            {
                return;
            }
            ValueField.ResignFirstResponder();
            _EntryCell.SendCompleted();
            _hasFocus = false;
        }

        void EntryCell_Focused(object sender, EventArgs e)
        {
            ValueField.BecomeFirstResponder();
        }


        bool OnShouldReturn(UITextField view)
        {
            _hasFocus = false;
            ValueField.ResignFirstResponder();
            _EntryCell.SendCompleted();
            return true;
        }
    }
}
