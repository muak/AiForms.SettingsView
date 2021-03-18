using System;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;
using Exception = System.Exception;
using Object = Java.Lang.Object;
using String = System.String;

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
        NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell;
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
            _min = _NumberPickerCell.Min;
        }

        void UpdateMax()
        {
            _max = _NumberPickerCell.Max;
        }

        void UpdateNumber()
        {
            vValueLabel.Text = FormatNumber(_NumberPickerCell.Number);
        }

        private string FormatNumber(int? number)
        {
            return number.HasValue && !String.IsNullOrEmpty(_NumberPickerCell.Unit)
                ? $"{number} {_NumberPickerCell.Unit}"
                : number?.ToString() ?? "";
        }

        void UpdatePickerTitle()
        {
            _title = _NumberPickerCell.PickerTitle;
        }

        void UpdateCommand()
        {
            _command = _NumberPickerCell.SelectedCommand;
        }

        void CreateDialog()
        {
            _picker = new APicker(_context);
            _picker.MinValue = _min;
            _picker.MaxValue = _max;
            if (_NumberPickerCell.Number.HasValue)
            {
                _picker.Value = _NumberPickerCell.Number.Value;
            }

            if (!String.IsNullOrEmpty(_NumberPickerCell.Unit))
            {
                _picker.SetFormatter(new UnitFormatter(_NumberPickerCell.Unit));
                ApplyInitialFormattingBugfix(_picker);
            }

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
                        _NumberPickerCell.Number = _picker.Value;
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

        // see bug https://stackoverflow.com/questions/17708325/android-numberpicker-with-formatter-doesnt-format-on-first-rendering/54083214#54083214
        // and https://issuetracker.google.com/issues/36952035
        private static void ApplyInitialFormattingBugfix(APicker picker)
        {
            try
            {
                Class klass = Java.Lang.Class.FromType(typeof(NumberPicker));
                Field f = klass.GetDeclaredField("mInputText");
                f.Accessible = true;
                EditText inputText = (EditText) f.Get(picker);
                inputText.SetFilters(new IInputFilter[0]);
            }
            catch (Exception)
            {
                // silently ignore this
            }
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
    
    internal class UnitFormatter : Object, NumberPicker.IFormatter
    {
        private readonly string _unit;

        public UnitFormatter(string unit)
        {
            _unit = unit;
        }

        public string Format(int value)
        {
            return value + " " + _unit;
        }
    }
}
