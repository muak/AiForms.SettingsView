using AiForms.Renderers;
using AiForms.Renderers.iOS;
using UIKit;
using Xamarin.Forms;
using System.Collections.Specialized;
using System;
using Foundation;
using System.Linq;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
	/// <summary>
	/// Picker cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	/// <summary>
	/// Picker cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView
	{
		PickerCell _PickerCell => Cell as PickerCell;
		PickerTableViewController _pickerVC;
		INotifyCollectionChanged _notifyCollection;
		INotifyCollectionChanged _selectedCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.PickerCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public PickerCellView(Cell formsCell) : base(formsCell)
		{
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			SetRightMarginZero();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
				e.PropertyName == PickerCell.SelectedItemProperty.PropertyName ||
				e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
				e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
				e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName )
			{
				UpdateSelectedItems();
			}
			if ( e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName )
			{
				if ( _PickerCell.UseAutoValueText )
				{
					UpdateSelectedItems();
				}
				else
				{
					base.UpdateValueText();
				}
			}
			if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems();
			}
			else if ( e.PropertyName == CellBase.ValueTextAlignmentProperty.PropertyName )
			{
				UpdateValueTextAlignment();
			}
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if ( _PickerCell.ItemsSource == null )
			{
				tableView.DeselectRow(indexPath, true);
				return;
			}

			var naviCtrl = GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
			_pickerVC?.Dispose();
			_pickerVC = new PickerTableViewController(this, tableView);
			BeginInvokeOnMainThread(() => naviCtrl.PushViewController(_pickerVC, true));

			if ( !_PickerCell.KeepSelectedUntilBack )
			{
				tableView.DeselectRow(indexPath, true);
			}
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateSelectedItems();
			UpdateCollectionChanged();
			UpdateValueTextAlignment();
		}

		/// <summary>
		/// Updates the selected items.
		/// </summary>
		/// <param name="force">If set to <c>true</c> force.</param>
		public void UpdateSelectedItems()
		{
			if ( !_PickerCell.UseAutoValueText )
			{
				return;
			}

			if ( _selectedCollection != null )
			{
				_selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
			}

			_selectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _selectedCollection != null )
			{
				_selectedCollection.CollectionChanged += SelectedItems_CollectionChanged;
			}

			ValueLabel.Text = _PickerCell.GetSelectedItemsText();
		}

		protected override void UpdateAllowMultiLine()
		{
			base.UpdateAllowMultiLine();
			ValueLabel.Lines = _CellBase.AllowMultiLine ? 1 : _CellBase.MaxLines;
		}
		void UpdateValueTextAlignment()
		{
			ValueLabel.TextAlignment = GetTextAllignment(_CellBase.ValueTextAlignment);
		}
		void UpdateCollectionChanged()
		{
			if ( _notifyCollection != null )
			{
				_notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
			}

			_notifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

			if ( _notifyCollection != null )
			{
				_notifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
				ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		/// <summary>
		/// Updates the is enabled.
		/// </summary>
		protected override void UpdateIsEnabled()
		{
			if ( _PickerCell.ItemsSource != null && _PickerCell.ItemsSource.Count == 0 )
			{
				return;
			}
			base.UpdateIsEnabled();
		}

		void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if ( !_CellBase.IsEnabled )
			{
				return;
			}

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateSelectedItems();
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
				_pickerVC?.Dispose();
				_pickerVC = null;

				if ( _notifyCollection != null )
				{
					_notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_notifyCollection = null;
				}
				if ( _selectedCollection != null )
				{
					_selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_selectedCollection = null;
				}
			}
			base.Dispose(disposing);
		}

		// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
		UINavigationController GetUINavigationController(UIViewController controller)
		{
			if ( controller != null )
			{
				if ( controller.PresentedViewController != null )
				{
					// on modal page
					return GetUINavigationController(controller.PresentedViewController);
				}
				if ( controller is UINavigationController )
				{
					return ( controller as UINavigationController );
				}
				if ( controller is UITabBarController )
				{
					//in case Root->Tab->Navi->Page
					var tabCtrl = controller as UITabBarController;
					return GetUINavigationController(tabCtrl.SelectedViewController);
				}
				if ( controller.ChildViewControllers.Count() != 0 )
				{
					var count = controller.ChildViewControllers.Count();

					for ( int c = 0; c < count; c++ )
					{
						var child = GetUINavigationController(controller.ChildViewControllers[c]);
						if ( child == null )
						{
							//TODO: Analytics...
						}
						else if ( child is UINavigationController )
						{
							return ( child as UINavigationController );
						}
					}
				}
			}

			return null;
		}

	}
}
