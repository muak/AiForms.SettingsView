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
using Android.Text.Method;
using Android.Graphics;
using Android.Runtime;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(AiForms.Renderers.Droid.EntryCellRenderer))]
namespace AiForms.Renderers.Droid
{
	/// <summary>
	/// Entry cell renderer.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	/// <summary>
	/// Entry cell view.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class EntryCellView : CellBaseView, ITextWatcher,
		TextView.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		AiEntryCell _EntryCell => Cell as AiEntryCell;

		AiEditText _editText;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.EntryCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public EntryCellView(Context context, Cell cell) : base(context, cell)
		{
			_editText = new AiEditText(context);

			_editText.Focusable = true;
			_editText.ImeOptions = ImeAction.Done;
			_editText.SetOnEditorActionListener(this);

			_editText.OnFocusChangeListener = this;
			_editText.SetSingleLine(true);
			_editText.Ellipsize = TextUtils.TruncateAt.End;

			_editText.InputType |= InputTypes.TextFlagNoSuggestions;  //disabled spell check
			_editText.Background.Alpha = 0;  //hide underline

			_editText.ClearFocusAction = DoneEdit;
			Click += EntryCellView_Click;

			_EntryCell.Focused += EntryCell_Focused;

			//remove weight and change width due to fill _EditText.
			var titleParam = TitleLabel.LayoutParameters as LinearLayout.LayoutParams;
			titleParam.Weight = 0;
			titleParam.Width = ViewGroup.LayoutParams.WrapContent;
			titleParam = null;

			var lparams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);

			using ( lparams )
			{
				ContentStack.AddView(_editText, lparams);
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
			UpdateIsPassword();
			UpdateValueTextAlignment();
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
			if ( e.PropertyName == AiEntryCell.ValueTextProperty.PropertyName )
			{
				UpdateValueText();
			}
			else if ( e.PropertyName == AiEntryCell.ValueTextFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateValueTextFontSize);
			}
			else if ( e.PropertyName == AiEntryCell.ValueTextColorProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateValueTextColor);
			}
			else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName )
			{
				UpdateKeyboard();
			}
			else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName )
			{
				UpdatePlaceholder();
			}
			else if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
			}
			else if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName )
			{
				UpdateTextAlignment();
			}
			else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName )
			{
				UpdateIsPassword();
			}
			else if ( e.PropertyName == AiEntryCell.ValueTextAlignmentProperty.PropertyName )
			{
				UpdateValueTextAlignment();
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
			if ( e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
			}
			else if ( e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateValueTextFontSize);
			}
			else if ( e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
			}
			else if ( e.PropertyName == CellBase.ValueTextAlignmentProperty.PropertyName )
			{
				UpdateValueTextAlignment();
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing )
			{
				Click -= EntryCellView_Click;
				_EntryCell.Focused -= EntryCell_Focused;
				_editText.RemoveFromParent();
				_editText.SetOnEditorActionListener(null);
				_editText.RemoveTextChangedListener(this);
				_editText.OnFocusChangeListener = null;
				_editText.ClearFocusAction = null;
				_editText.Dispose();
				_editText = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance(bool isEnabled)
		{
			if ( isEnabled )
			{
				_editText.Enabled = true;
				_editText.Alpha = 1.0f;
			}
			else
			{
				_editText.Enabled = false;
				_editText.Alpha = 0.3f;
			}
			base.SetEnabledAppearance(isEnabled);
		}

		void EntryCellView_Click(object sender, EventArgs e)
		{
			_editText.RequestFocus();
			if ( _EntryCell.SelectAllOnTap ) _editText.SelectAll();
			ShowKeyboard(_editText);
		}



		void UpdateValueTextAlignment()
		{
			_editText.TextAlignment = GetTextAllignment(_CellBase.ValueTextAlignment);
		}


		void UpdateValueText()
		{
			_editText.RemoveTextChangedListener(this);
			if ( _editText.Text != _EntryCell.ValueText )
			{
				_editText.Text = _EntryCell.ValueText;
			}
			_editText.AddTextChangedListener(this);
		}

		void UpdateValueTextFontSize()
		{
			if ( _EntryCell.ValueTextFontSize > 0 )
			{
				_editText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _EntryCell.ValueTextFontSize);
			}
			else if ( CellParent != null )
			{
				_editText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize);
			}
		}

		void UpdateValueTextColor()
		{
			if ( _EntryCell.ValueTextColor != Xamarin.Forms.Color.Default )
			{
				_editText.SetTextColor(_EntryCell.ValueTextColor.ToAndroid());
			}
			else if ( CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default )
			{
				_editText.SetTextColor(CellParent.CellValueTextColor.ToAndroid());
			}
		}

		void UpdateKeyboard()
		{
			_editText.InputType = _EntryCell.Keyboard.ToInputType() | InputTypes.TextFlagNoSuggestions;
		}

		void UpdateIsPassword()
		{
			_editText.TransformationMethod = _EntryCell.IsPassword ? new PasswordTransformationMethod() : null;

		}

		void UpdatePlaceholder()
		{
			_editText.Hint = _EntryCell.Placeholder;
			_editText.SetHintTextColor(Android.Graphics.Color.Rgb(210, 210, 210));
		}

		void UpdateTextAlignment()
		{
			_editText.Gravity = _EntryCell.TextAlignment.ToGravityFlags();
		}

		void UpdateAccentColor()
		{
			if ( _EntryCell.AccentColor != Xamarin.Forms.Color.Default )
			{
				ChangeTextViewBack(_EntryCell.AccentColor.ToAndroid());
			}
			else if ( CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default )
			{
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
			_editText.Background.SetTintList(colorlist);
		}


		bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, Android.Views.KeyEvent e)
		{
			if ( actionId == ImeAction.Done ||
					( actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter ) )
			{
				HideKeyboard(v);
				DoneEdit();
			}

			return true;
		}

		void DoneEdit()
		{
			var entryCell = (IEntryCellController) Cell;
			//entryCell.SendCompleted();
			_editText.ClearFocus();
			ClearFocus();
		}

		void HideKeyboard(Android.Views.View inputView)
		{
			using ( var inputMethodManager = (InputMethodManager) _context.GetSystemService(Context.InputMethodService) )
			{
				IBinder windowToken = inputView.WindowToken;
				if ( windowToken != null )
					inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
			}
		}
		void ShowKeyboard(Android.Views.View inputView)
		{
			using ( var inputMethodManager = (InputMethodManager) _context.GetSystemService(Context.InputMethodService) )
			{

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
			if ( string.IsNullOrEmpty(_EntryCell.ValueText) && s.Length() == 0 )
			{
				return;
			}

			_EntryCell.ValueText = s?.ToString();
		}

		void IOnFocusChangeListener.OnFocusChange(Android.Views.View v, bool hasFocus)
		{
			if ( hasFocus )
			{
				//show underline when on focus.
				_editText.Background.Alpha = 100;
			}
			else
			{
				//hide underline
				_editText.Background.Alpha = 0;
				// consider as text inpute completed.
				_EntryCell.SendCompleted();
			}
		}

		void EntryCell_Focused(object sender, EventArgs e)
		{
			_editText.RequestFocus();
			ShowKeyboard(_editText);
		}

	}

	[Android.Runtime.Preserve(AllMembers = true)]
	internal class AiEditText : EditText
	{
		public Action ClearFocusAction { get; set; }
		SoftInput _startingMode;

		public AiEditText(Context context) : base(context)
		{
		}

		public override bool OnKeyPreIme(Keycode keyCode, KeyEvent e)
		{
			if ( keyCode == Keycode.Back && e.Action == KeyEventActions.Up )
			{
				ClearFocus();
				ClearFocusAction?.Invoke();
			}
			return base.OnKeyPreIme(keyCode, e);

		}
	}
}
