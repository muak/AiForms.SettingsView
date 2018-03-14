using System;
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
    /// Number picker cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

    /// <summary>
    /// Number picker cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class TextPickerCellView : LabelCellView, IPickerCell
    {
        TextPickerCell _TextPickerCell => Cell as TextPickerCell;
        APicker _picker;
        AlertDialog _dialog;
        Context _context;
        string _title;
        ICommand _command;
        int _max;
        int _min;
        string[] _displayValues;

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
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdatePickerTitle();
            UpdatePickerItems();
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

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void ShowDialog()
        {
            CreateDialog();
        }

        void UpdateSelectedItem()
        {
            vValueLabel.Text = _TextPickerCell.SelectedItem.ToString();
        }

        void UpdatePickerTitle()
        {
            _title = _TextPickerCell.PickerTitle;
        }

        void UpdatePickerItems()
        {
            _min = 0;
            _max = _TextPickerCell.Items.Count() - 1;
            _displayValues = _TextPickerCell.Items.ToArray();
        }

        void UpdateCommand()
        {
            _command = _TextPickerCell.SelectedCommand;
        }

        void CreateDialog()
        {
            _picker = new APicker(_context);
            _picker.MinValue = _min;
            _picker.MaxValue = _max;
            _picker.SetDisplayedValues(_displayValues);
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
                        _TextPickerCell.SelectedItem = _displayValues[_picker.Value];
                        _command?.Execute(_picker.Value);
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


























//using System;
//using System.Windows.Input;
//using AiForms.Renderers;
//using AiForms.Renderers.Droid;
//using Android.App;
//using Android.Content;
//using Android.Views;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using AListView = Android.Widget.ListView;

//[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]
//namespace AiForms.Renderers.Droid
//{
//    /// <summary>
//    /// Number picker cell renderer.
//    /// </summary>
//    [Android.Runtime.Preserve(AllMembers = true)]
//    public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

//    /// <summary>
//    /// Number picker cell view.
//    /// </summary>
//    [Android.Runtime.Preserve(AllMembers = true)]
//    public class TextPickerCellView : LabelCellView, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
//    {
//        TextPickerCell _textPickerCell => Cell as TextPickerCell;
//        AListView _listView;
//        TextPickerAdapter _adapter;
//        AlertDialog _dialog;
//        Context _context;
//        string _title;
//        ICommand _command;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.TextPickerCellView"/> class.
//        /// </summary>
//        /// <param name="context">Context.</param>
//        /// <param name="cell">Cell.</param>
//        public TextPickerCellView(Context context, Cell cell) : base(context, cell)
//        {
//            _context = context;
//        }

//        /// <summary>
//        /// Cells the property changed.
//        /// </summary>
//        /// <param name="sender">Sender.</param>
//        /// <param name="e">E.</param>
//        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            base.CellPropertyChanged(sender, e);
//            if (e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName) {
//                UpdateSelectedItem();
//            }
//            else if (e.PropertyName == TextPickerCell.PickerTitleProperty.PropertyName) {
//                UpdatePickerTitle();
//            }
//            else if (e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName) {
//                UpdateCommand();
//            }
//        }

//        /// <summary>
//        /// Updates the cell.
//        /// </summary>
//        public override void UpdateCell()
//        {
//            base.UpdateCell();
//            UpdatePickerTitle();
//            UpdateSelectedItem();
//            UpdateCommand();
//        }

//        /// <summary>
//        /// Dispose the specified disposing.
//        /// </summary>
//        /// <returns>The dispose.</returns>
//        /// <param name="disposing">If set to <c>true</c> disposing.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing) {
//                _listView?.Dispose();
//                _listView = null;
//                _dialog?.Dispose();
//                _dialog = null;
//                _adapter?.Dispose();
//                _adapter = null;
//                _context = null;
//                _command = null;
//            }
//            base.Dispose(disposing);
//        }

//        /// <summary>
//        /// Shows the dialog.
//        /// </summary>
//        public void ShowDialog()
//        {
//            CreateDialog();
//        }

//        void UpdateSelectedItem()
//        {
//            vValueLabel.Text = _textPickerCell.SelectedItem.ToString();
//        }

//        void UpdatePickerTitle()
//        {
//            _title = _textPickerCell.PickerTitle;
//        }

//        void UpdateCommand()
//        {
//            _command = _textPickerCell.SelectedCommand;
//        }

//        void CreateDialog()
//        {
//            _listView = new AListView(_context);
//            _listView.Focusable = false;
//            _listView.DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
//            _listView.SetDrawSelectorOnTop(true);
//            _listView.ChoiceMode = Android.Widget.ChoiceMode.Single;
//            _adapter = new TextPickerAdapter(_context, _textPickerCell, _listView);
//            _listView.OnItemClickListener = _adapter;
//            _listView.Adapter = _adapter;

//            _adapter.CloseAction = () => {
//                _dialog.GetButton((int)DialogButtonType.Positive).PerformClick();
//            };

//            if (_dialog == null) {
//                using (var builder = new AlertDialog.Builder(_context)) {
//                    builder.SetTitle(_textPickerCell.PageTitle);
//                    builder.SetView(_listView);

//                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) => {
//                        ClearFocus();
//                    });
//                    builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) => {
//                        _adapter.DoneSelect();
//                        UpdateSelectedItem();
//                        _textPickerCell.InvokeCommand();
//                        ClearFocus();
//                    });


//                    _dialog = builder.Create();
//                }

//                _dialog.SetCanceledOnTouchOutside(true);
//                _dialog.SetOnDismissListener(this);
//                _dialog.SetOnShowListener(this);
//                _dialog.Show();
//            }
//        }

//        /// <summary>
//        /// Ons the show.
//        /// </summary>
//        /// <param name="dialog">Dialog.</param>
//        public void OnShow(IDialogInterface dialog)
//        {
//            _adapter.RestoreSelect();
//        }

//        /// <summary>
//        /// Ons the dismiss.
//        /// </summary>
//        /// <param name="dialog">Dialog.</param>
//        public void OnDismiss(IDialogInterface dialog)
//        {
//            _dialog.SetOnShowListener(null);
//            _dialog.SetOnDismissListener(null);
//            _dialog.Dispose();
//            _dialog = null;
//            _adapter?.Dispose();
//            _adapter = null;
//            _listView.Dispose();
//            _listView = null;
//            this.Selected = false;
//        }
//    }
//}
