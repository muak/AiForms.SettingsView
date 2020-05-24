using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]
namespace AiForms.Renderers.iOS
{
	/// <summary>
	/// Radio cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	/// <summary>
	/// Radio cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class RadioCellView : CellBaseView
	{
		RadioCell _RadioCell => Cell as RadioCell;

		private object _SelectedValue
		{
			get
			{
				return RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			}
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) != null )
				{
					RadioCell.SetSelectedValue(_RadioCell.Section, value);
				}
				else
				{
					RadioCell.SetSelectedValue(CellParent, value);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.RadioCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public RadioCellView(Cell formsCell) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Default;
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
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
			if ( e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
			}
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName )
			{
				UpdateSelectedValue();
			}
		}

		/// <summary>
		/// Sections the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.SectionPropertyChanged(sender, e);
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName )
			{
				UpdateSelectedValue();
			}
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if ( Accessory == UITableViewCellAccessory.None )
			{
				_SelectedValue = _RadioCell.Value;
			}
			tableView.DeselectRow(indexPath, true);
		}

		void UpdateSelectedValue()
		{
			bool result;
			if ( _RadioCell.Value.GetType().IsValueType )
			{
				result = object.Equals(_RadioCell.Value, _SelectedValue);
			}
			else
			{
				result = object.ReferenceEquals(_RadioCell.Value, _SelectedValue);
			}

			Accessory = result ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
		}

		void UpdateAccentColor()
		{
			if ( !_RadioCell.AccentColor.IsDefault )
			{
				TintColor = _RadioCell.AccentColor.ToUIColor();
			}
			else if ( CellParent != null && !CellParent.CellAccentColor.IsDefault )
			{
				TintColor = CellParent.CellAccentColor.ToUIColor();
			}
		}
	}
}
