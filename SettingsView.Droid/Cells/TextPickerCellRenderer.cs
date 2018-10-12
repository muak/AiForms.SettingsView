using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Text picker cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

    /// <summary>
    /// Text picker cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class TextPickerCellView : LabelCellView
    {
        TextPickerCell _TextPickerCell => Cell as TextPickerCell;
        APicker _picker;
        AlertDialog _dialog;
        Context _context;
        string _title;
        ICommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.TextPickerCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public TextPickerCellView(Context context, Cell cell) : base(context, cell)
        {
            _context = context;
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName) {
                UpdateSelectedItem();
            }
            else if (e.PropertyName == TextPickerCell.PickerTitleProperty.PropertyName) {
                UpdatePickerTitle();
            }
            else if (e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="adapter">Adapter.</param>
        /// <param name="position">Position.</param>
        public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
        {
            CreateDialog();
        }


        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdatePickerTitle();
            UpdateSelectedItem();
            UpdateCommand();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _picker?.Dispose();
                _picker = null;
                _dialog?.Dispose();
                _dialog = null;
                _context = null;
                _command = null;
            }
            base.Dispose(disposing);
        }

        void UpdateSelectedItem()
        {
            vValueLabel.Text = _TextPickerCell.SelectedItem?.ToString();
        }

        void UpdatePickerTitle()
        {
            _title = _TextPickerCell.PickerTitle;
        }

        void UpdateCommand()
        {
            _command = _TextPickerCell.SelectedCommand;
        }

        void CreateDialog()
        {
            if(_TextPickerCell.Items == null || _TextPickerCell.Items.Count == 0){
                return;
            }

            var displayValues = _TextPickerCell.Items.Cast<object>().Select(x => x.ToString()).ToArray();

            _picker = new APicker(_context);
            _picker.MinValue = 0;
            _picker.MaxValue = _TextPickerCell.Items.Count - 1;
            _picker.SetDisplayedValues(displayValues);
            _picker.Value = _TextPickerCell.Items.IndexOf(_TextPickerCell.SelectedItem);

            if (_dialog == null) {
                using (var builder = new AlertDialog.Builder(_context)) {

                    builder.SetTitle(_title);

                    Android.Widget.FrameLayout parent = new Android.Widget.FrameLayout(_context);
                    parent.AddView(_picker, new Android.Widget.FrameLayout.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent,
                           GravityFlags.Center));
                    builder.SetView(parent);


                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) => {
                        ClearFocus();
                    });
                    builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) => {
                        _TextPickerCell.SelectedItem = _TextPickerCell.Items[_picker.Value];
                        _command?.Execute(_TextPickerCell.Items[_picker.Value]);
                        ClearFocus();
                    });

                    _dialog = builder.Create();
                }
                _dialog.SetCanceledOnTouchOutside(true);
                _dialog.DismissEvent += (ss, ee) => {
                    _dialog.Dispose();
                    _dialog = null;
                    _picker.RemoveFromParent();
                    _picker.Dispose();
                    _picker = null;
                };

                _dialog.Show();
            }

        }
    }
}
