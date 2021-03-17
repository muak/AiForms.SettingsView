using System;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Number picker cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }

    /// <summary>
    /// Number picker cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class NumberPickerCellView : LabelCellView
    {
        NumberPickerCell _NumberPikcerCell => Cell as NumberPickerCell;
        APicker _picker;
        AlertDialog _dialog;
        Context _context;
        string _title;
        ICommand _command;
        int _max;
        int _min;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.NumberPickerCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public NumberPickerCellView(Context context, Cell cell) : base(context, cell)
        {
            _context = context;
        }

        public NumberPickerCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == NumberPickerCell.MinProperty.PropertyName) {
                UpdateMin();
            }
            else if (e.PropertyName == NumberPickerCell.MaxProperty.PropertyName) {
                UpdateMax();
            }
            else if (e.PropertyName == NumberPickerCell.NumberProperty.PropertyName) {
                UpdateNumber();
            }
            else if (e.PropertyName == NumberPickerCell.PickerTitleProperty.PropertyName) {
                UpdatePickerTitle();
            }
            else if (e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName) {
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
            UpdateMin();
            UpdateMax();
            UpdatePickerTitle();
            UpdateNumber();
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
                DestroyDialog();
                _context = null;
                _command = null;
            }
            base.Dispose(disposing);
        }

        void UpdateMin()
        {
            _min = _NumberPikcerCell.Min;
        }

        void UpdateMax()
        {
            _max = _NumberPikcerCell.Max;
        }

        void UpdateNumber()
        {
            vValueLabel.Text = FormatNumber(_NumberPikcerCell.Number);
        }

        protected virtual string FormatNumber(int? number)
        {
            return number?.ToString() ?? "";
        }

        void UpdatePickerTitle()
        {
            _title = _NumberPikcerCell.PickerTitle;
        }

        void UpdateCommand()
        {
            _command = _NumberPikcerCell.SelectedCommand;
        }

        void CreateDialog()
        {
            _picker = new APicker(_context);
            _picker.MinValue = _min;
            _picker.MaxValue = _max;
            if (_NumberPikcerCell.Number.HasValue)
            {
                _picker.Value = _NumberPikcerCell.Number.Value;
            }

            OnPickerCreated(_picker);

            if (_dialog == null) {
                using (var builder = new AlertDialog.Builder(_context)) {

                    builder.SetTitle(_title);

                    Android.Widget.FrameLayout parent = new Android.Widget.FrameLayout(_context);
                    parent.AddView(_picker, new Android.Widget.FrameLayout.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent,
                           GravityFlags.Center));
                    builder.SetView(parent);


                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) =>
                    {
                        ClearFocus();
                    });
                    builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) =>
                    {
                        _NumberPikcerCell.Number = _picker.Value;
                        _command?.Execute(_picker.Value);
                        ClearFocus();
                    });

                    _dialog = builder.Create();
                }
                _dialog.SetCanceledOnTouchOutside(true);
                _dialog.DismissEvent += (ss, ee) =>
                {
                    DestroyDialog();
                };

                _dialog.Show();
            }

        }

        protected virtual void OnPickerCreated(APicker picker)
        {
            // can be overwritten in subclasses
        }

        void DestroyDialog()
        {
            if (_dialog != null)
            {
                // Set _dialog to null to avoid racing attempts to destroy dialog - e.g. in response to dismiss event
                var dialog = _dialog;
                _dialog = null;
                _picker.RemoveFromParent();
                _picker.Dispose();
                _picker = null;
                // Dialog.Dispose() does not close an open dialog view so explicitly dismiss it before disposing
                dialog.Dismiss();
                dialog.Dispose();
            }
        }
    }
}
