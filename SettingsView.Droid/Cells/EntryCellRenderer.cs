using System;
using AiForms.Renderers.Droid.Extensions;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiEntryCell = AiForms.Renderers.EntryCell;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(AiForms.Renderers.Droid.EntryCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Entry cell renderer.
    /// </summary>
    public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

    /// <summary>
    /// Entry cell view.
    /// </summary>
    public class EntryCellView : CellBaseView, ITextWatcher,
        TextView.IOnFocusChangeListener, TextView.IOnEditorActionListener
    {
        AiEntryCell _EntryCell => Cell as AiEntryCell;

        AiEditText _EditText;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.EntryCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public EntryCellView(Context context, Cell cell) : base(context, cell)
        {
            _EditText = new AiEditText(context);

            _EditText.Focusable = true;
            _EditText.ImeOptions = ImeAction.Done;
            _EditText.SetOnEditorActionListener(this);

            _EditText.OnFocusChangeListener = this;
            _EditText.SetSingleLine(true);
            _EditText.Ellipsize = TextUtils.TruncateAt.End;

            _EditText.InputType |= InputTypes.TextFlagNoSuggestions;  //disabled spell check
            _EditText.Background.Alpha = 0;  //hide underline

            _EditText.ClearFocusAction = DoneEdit;
            Click += EntryCellView_Click;

            //remove weight and change width due to fill _EditText.
            var titleParam = TitleLabel.LayoutParameters as LinearLayout.LayoutParams;
            titleParam.Weight = 0;
            titleParam.Width = ViewGroup.LayoutParams.WrapContent;
            titleParam = null;

            var lparams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);

            using (lparams) {
                ContentStack.AddView(_EditText, lparams);
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
            UpdateKeyboard();
            UpdatePlaceholder();
            UpdateAccentColor();
            UpdateTextAlignment();
            base.UpdateCell();
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
                UpdateWithForceLayout(UpdateValueTextColor);
            }
            else if (e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName) {
                UpdateKeyboard();
            }
            else if (e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName) {
                UpdatePlaceholder();
            }
            else if (e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName) {
                UpdateAccentColor();
            }
            else if (e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName) {
                UpdateTextAlignment();
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
            }
            else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateValueTextFontSize);
            }
            else if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName) {
                UpdateAccentColor();
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
                Click -= EntryCellView_Click;
                _EditText.RemoveFromParent();
                _EditText.SetOnEditorActionListener(null);
                _EditText.RemoveTextChangedListener(this);
                _EditText.OnFocusChangeListener = null;
                _EditText.ClearFocusAction = null;
                _EditText.Dispose();
                _EditText = null;
            }
            base.Dispose(disposing);
        }

        protected override void SetEnabledAppearance(bool isEnabled)
        {
            if (isEnabled) {
                _EditText.Enabled = true;
                _EditText.Alpha = 1.0f;
            }
            else {
                _EditText.Enabled = false;
                _EditText.Alpha = 0.3f;
            }
            base.SetEnabledAppearance(isEnabled);
        }

        void EntryCellView_Click(object sender, EventArgs e)
        {
            _EditText.RequestFocus();
            ShowKeyboard(_EditText);
        }

        void UpdateValueText()
        {
            _EditText.RemoveTextChangedListener(this);
            if (_EditText.Text != _EntryCell.ValueText) {
                _EditText.Text = _EntryCell.ValueText;
            }
            _EditText.AddTextChangedListener(this);
        }

        void UpdateValueTextFontSize()
        {
            if (_EntryCell.ValueTextFontSize > 0) {
                _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_EntryCell.ValueTextFontSize);
            }
            else if (CellParent != null) {
                _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)CellParent.CellValueTextFontSize);
            }
        }

        void UpdateValueTextColor()
        {
            if (_EntryCell.ValueTextColor != Xamarin.Forms.Color.Default) {
                _EditText.SetTextColor(_EntryCell.ValueTextColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default) {
                _EditText.SetTextColor(CellParent.CellValueTextColor.ToAndroid());
            }
        }

        void UpdateKeyboard()
        {
            _EditText.InputType = _EntryCell.Keyboard.ToInputType() | InputTypes.TextFlagNoSuggestions;
        }

        void UpdatePlaceholder()
        {
            _EditText.Hint = _EntryCell.Placeholder;
            _EditText.SetHintTextColor(Android.Graphics.Color.Rgb(210, 210, 210));
        }

        void UpdateTextAlignment()
        {
            _EditText.Gravity = _EntryCell.TextAlignment.ToGravityFlags();
        }

        void UpdateAccentColor()
        {
            if (_EntryCell.AccentColor != Xamarin.Forms.Color.Default) {
                ChangeTextViewBack(_EntryCell.AccentColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default) {
                ChangeTextViewBack(CellParent.CellAccentColor.ToAndroid());
            }
        }

        void ChangeTextViewBack(Android.Graphics.Color accent)
        {
            var colorlist = new ColorStateList(new int[][]
            {
                new int[]{global::Android.Resource.Attribute.StateFocused},
                new int[]{-global::Android.Resource.Attribute.StateFocused},
            },
                new int[] {
                    Android.Graphics.Color.Argb(255,accent.R,accent.G,accent.B),
                    Android.Graphics.Color.Argb(255, 200, 200, 200)
            });
            _EditText.Background.SetTintList(colorlist);
        }


        bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, Android.Views.KeyEvent e)
        {
            if (actionId == ImeAction.Done ||
                    (actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter)) {
                HideKeyboard(v);
                DoneEdit();
            }

            return true;
        }

        void DoneEdit()
        {
            var entryCell = (IEntryCellController)Cell;
            entryCell.SendCompleted();
            _EditText.ClearFocus();
            ClearFocus();
        }

        void HideKeyboard(Android.Views.View inputView)
        {
            using (var inputMethodManager = (InputMethodManager)_Context.GetSystemService(Context.InputMethodService)) {
                IBinder windowToken = inputView.WindowToken;
                if (windowToken != null)
                    inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
            }
        }
        void ShowKeyboard(Android.Views.View inputView)
        {
            using (var inputMethodManager = (InputMethodManager)_Context.GetSystemService(Context.InputMethodService)) {

                inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
                inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);

            }
        }

        void ITextWatcher.AfterTextChanged(IEditable s)
        {
        }

        void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            _EntryCell.ValueText = s?.ToString();
        }

        void IOnFocusChangeListener.OnFocusChange(Android.Views.View v, bool hasFocus)
        {
            if (hasFocus) {
                //show underline when on focus.
                _EditText.Background.Alpha = 100;
            }
            else {
                //hide underline
                _EditText.Background.Alpha = 0;
            }
        }
    }

    internal class AiEditText : EditText
    {
        public Action ClearFocusAction { get; set; }
        public AiEditText(Context context) : base(context)
        {
        }

        public override bool OnKeyPreIme(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && e.Action == KeyEventActions.Up) {
                ClearFocus();
                ClearFocusAction?.Invoke();
            }
            return base.OnKeyPreIme(keyCode, e);

        }
    }
}
