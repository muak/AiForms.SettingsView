using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AiForms.Renderers.SwitchCell), typeof(AiForms.Renderers.Droid.SwitchCellRenderer))]
namespace AiForms.Renderers.Droid
{
	/// <summary>
	/// Switch cell renderer.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	/// <summary>
	/// Switch cell view.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
	{
		SwitchCompat _Switch { get; set; }
		SwitchCell _SwitchCell => Cell as SwitchCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SwitchCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public SwitchCellView(Context context, Cell cell) : base(context, cell)
		{

			_Switch = new SwitchCompat(context);

			_Switch.SetOnCheckedChangeListener(this);
			_Switch.Gravity = Android.Views.GravityFlags.Right;

			var switchParam = new LinearLayout.LayoutParams(
				ViewGroup.LayoutParams.WrapContent,
				ViewGroup.LayoutParams.WrapContent)
			{
			};

			using ( switchParam )
			{
				AccessoryStack.AddView(_Switch, switchParam);
			}

			_Switch.Focusable = false;
			Focusable = false;
			DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
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
			if ( e.PropertyName == SwitchCell.AccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
			}
			if ( e.PropertyName == SwitchCell.OnProperty.PropertyName )
			{
				UpdateOn();
			}
			else if ( e.PropertyName == CellBase.ValueTextAlignmentProperty.PropertyName )
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
			if ( e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
			}
		}

		/// <summary>
		/// Ons the checked changed.
		/// </summary>
		/// <param name="buttonView">Button view.</param>
		/// <param name="isChecked">If set to <c>true</c> is checked.</param>
		public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			_SwitchCell.On = isChecked;
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
		{
			_Switch.Checked = !_Switch.Checked;
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
				_Switch.SetOnCheckedChangeListener(null);
				_Switch.Background?.Dispose();
				_Switch.Background = null;
				_Switch.ThumbDrawable?.Dispose();
				_Switch.ThumbDrawable = null;
				_Switch.Dispose();
				_Switch = null;
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
				_Switch.Enabled = true;
				_Switch.Alpha = 1.0f;
			}
			else
			{
				_Switch.Enabled = false;
				_Switch.Alpha = 0.3f;
			}
			base.SetEnabledAppearance(isEnabled);
		}

		void UpdateOn()
		{
			_Switch.Checked = _SwitchCell.On;
		}

		void UpdateValueTextAlignment()
		{
			_Switch.TextAlignment = GetTextAllignment(_CellBase.ValueTextAlignment);
		}
		void UpdateAccentColor()
		{
			if ( _SwitchCell.AccentColor != Xamarin.Forms.Color.Default )
			{
				ChangeSwitchColor(_SwitchCell.AccentColor.ToAndroid());
			}
			else if ( CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default )
			{
				ChangeSwitchColor(CellParent.CellAccentColor.ToAndroid());
			}
		}

		void ChangeSwitchColor(Android.Graphics.Color accent)
		{
			var trackColors = new ColorStateList(new int[][]
				  {
							new int[]{global::Android.Resource.Attribute.StateChecked},
							new int[]{-global::Android.Resource.Attribute.StateChecked},
				  },
				 new int[] {
							Android.Graphics.Color.Argb(76,accent.R,accent.G,accent.B),
							Android.Graphics.Color.Argb(76, 117, 117, 117)
				  });


			_Switch.TrackDrawable.SetTintList(trackColors);

			var thumbColors = new ColorStateList(new int[][]
				 {
							new int[]{global::Android.Resource.Attribute.StateChecked},
							new int[]{-global::Android.Resource.Attribute.StateChecked},
				 },
				new int[] {
							accent,
							Android.Graphics.Color.Argb(255, 244, 244, 244)
				 });

			_Switch.ThumbDrawable.SetTintList(thumbColors);

			var ripple = _Switch.Background as RippleDrawable;
			ripple.SetColor(trackColors);
		}
	}
}
