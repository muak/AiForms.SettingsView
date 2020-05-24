using System;
using System.Collections.Generic;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]
namespace AiForms.Renderers.iOS
{
	/// <summary>
	/// Text picker cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	/// <summary>
	/// Text picker cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellView : LabelCellView
	{
		/// <summary>
		/// Gets or sets the dummy field.
		/// </summary>
		/// <value>The dummy field.</value>
		public UITextField DummyField { get; set; }
		TextPickerSource _model;
		UILabel _titleLabel;
		UIPickerView _picker;
		ICommand _command;

		TextPickerCell _TextPickerCell => Cell as TextPickerCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.TextPickerCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public TextPickerCellView(Cell formsCell) : base(formsCell)
		{

			DummyField = new NoCaretField();
			DummyField.BorderStyle = UITextBorderStyle.None;
			DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpPicker();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName )
			{
				UpdateSelectedItem();
			}
			else if ( e.PropertyName == TextPickerCell.PickerTitleProperty.PropertyName )
			{
				UpdateTitle();
			}
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName )
			{
				UpdateCommand();
			}
			else if ( e.PropertyName == TextPickerCell.ItemsProperty.PropertyName )
			{
				UpdateItems();
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
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateItems();
			UpdateSelectedItem();
			UpdateTitle();
			UpdateCommand();
			UpdateValueTextAlignment();
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
				_model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
				DummyField.RemoveFromSuperview();
				DummyField?.Dispose();
				DummyField = null;
				_titleLabel?.Dispose();
				_titleLabel = null;
				_model?.Dispose();
				_model = null;
				_picker?.Dispose();
				_picker = null;
				_command = null;

			}
			base.Dispose(disposing);
		}

		void SetUpPicker()
		{
			_picker = new UIPickerView();

			var width = UIScreen.MainScreen.Bounds.Width;

			_titleLabel = new UILabel();
			_titleLabel.TextAlignment = UITextAlignment.Center;

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) =>
			{
				DummyField.ResignFirstResponder();
				Select(_model.PreSelectedItem);
			});

			var labelButton = new UIBarButtonItem(_titleLabel);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
			{
				_model.OnUpdatePickerFormModel();
				DummyField.ResignFirstResponder();
				_command?.Execute(_model.SelectedItem);
			});

			toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);

			DummyField.InputView = _picker;
			DummyField.InputAccessoryView = toolbar;

			_model = new TextPickerSource();
			_picker.Model = _model;

			_model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		void UpdateSelectedItem()
		{
			Select(_TextPickerCell.SelectedItem);
			ValueLabel.Text = _TextPickerCell.SelectedItem?.ToString();
		}

		void UpdateItems()
		{
			var items = _TextPickerCell.Items ?? new List<object>();
			_model.SetItems(items);
			// Force picker view to reload data from model after change
			// Otherwise it might access the model based on old view data
			// causing "Index was out of range" errors and the like.
			_picker.ReloadAllComponents();
			Select(_TextPickerCell.SelectedItem);
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
		void UpdateTitle()
		{
			_titleLabel.Text = _TextPickerCell.PickerTitle;
			_titleLabel.SizeToFit();
			_titleLabel.Frame = new CGRect(0, 0, 160, 44);
		}

		void UpdateCommand()
		{
			_command = _TextPickerCell.SelectedCommand;
		}

		void Model_UpdatePickerFromModel(object sender, EventArgs e)
		{
			_TextPickerCell.SelectedItem = _model.SelectedItem;
			ValueLabel.Text = _model.SelectedItem?.ToString();
		}

		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		void Select(object item)
		{
			var idx = _model.Items.IndexOf(item);
			if ( idx == -1 )
			{
				item = _model.Items.Count == 0 ? null : _model.Items[0];
				idx = 0;
			}
			_picker.Select(idx, 0, false);
			_model.SelectedItem = item;
			_model.SelectedIndex = idx;
			_model.PreSelectedItem = item;
		}
	}
}
