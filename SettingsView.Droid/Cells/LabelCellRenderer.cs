using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]
namespace AiForms.Renderers.Droid
{
	/// <summary>
	/// Label cell renderer.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	/// <summary>
	/// Label cell view.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class LabelCellView : CellBaseView
	{
		LabelCell _LabelCell => Cell as LabelCell;
		/// <summary>
		/// Gets or sets the value label.
		/// </summary>
		/// <value>The value label.</value>
		public TextView ValueLabel { get; set; }
		/// <summary>
		/// The v value label.
		/// </summary>
		public TextView vValueLabel;


		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.LabelCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public LabelCellView(Context context, Cell cell) : base(context, cell)
		{

			ValueLabel = new TextView(context);
			ValueLabel.SetSingleLine(true);
			ValueLabel.Ellipsize = TextUtils.TruncateAt.End;
			ValueLabel.Gravity = GravityFlags.Right;

			using var textParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			ContentStack.AddView(ValueLabel, textParams);
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == LabelCell.ValueTextProperty.PropertyName )
			{
				UpdateValueText();
			}
			else if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName )
			{
				UpdateValueTextFontSize();
			}
			else if ( e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
			}
			else if ( e.PropertyName == LabelCell.IgnoreUseDescriptionAsValueProperty.PropertyName )
			{
				UpdateUseDescriptionAsValue();
			}
			else if ( e.PropertyName == LabelCell.ValueTextAlignmentProperty.PropertyName )
			{
				UpdateValueTextAlignment();
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

			if ( e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
			}
			else if ( e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName )
			{
				UpdateValueTextFontSize();
			}
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateUseDescriptionAsValue();  //at first after base
			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFontSize();

		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance(bool isEnabled)
		{
			if ( isEnabled )
			{
				ValueLabel.Alpha = 1f;
			}
			else
			{
				ValueLabel.Alpha = 0.3f;
			}
			base.SetEnabledAppearance(isEnabled);
		}

		void UpdateUseDescriptionAsValue()
		{
			if ( !_LabelCell.IgnoreUseDescriptionAsValue && CellParent != null && CellParent.UseDescriptionAsValue )
			{
				vValueLabel = DescriptionLabel;
				DescriptionLabel.Visibility = ViewStates.Visible;
				ValueLabel.Visibility = ViewStates.Gone;
			}
			else
			{
				vValueLabel = ValueLabel;
				ValueLabel.Visibility = ViewStates.Visible;
			}
		}

		/// <summary>
		/// Updates the value text.
		/// </summary>
		protected void UpdateValueText()
		{
			vValueLabel.Text = _LabelCell.ValueText;
		}

		void UpdateValueTextAlignment()
		{
			ValueLabel.TextAlignment = GetTextAllignment(_CellBase.ValueTextAlignment);
		}


		void UpdateValueTextFontSize()
		{
			if ( _LabelCell.ValueTextFontSize > 0 )
			{
				ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _LabelCell.ValueTextFontSize);
			}
			else if ( CellParent != null )
			{
				ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize);
			}
			Invalidate();
		}

		void UpdateValueTextColor()
		{
			if ( _LabelCell.ValueTextColor != Xamarin.Forms.Color.Default )
			{
				ValueLabel.SetTextColor(_LabelCell.ValueTextColor.ToAndroid());
			}
			else if ( CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default )
			{
				ValueLabel.SetTextColor(CellParent.CellValueTextColor.ToAndroid());
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
				ValueLabel?.Dispose();
				ValueLabel = null;
				vValueLabel = null;
			}
			base.Dispose(disposing);
		}
	}
}
