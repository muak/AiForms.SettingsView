using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using AListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Picker cell renderer.
    /// </summary>
    public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

    /// <summary>
    /// Picker cell view.
    /// </summary>
    public class PickerCellView : LabelCellView, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
    {
        PickerCell _PickerCell => Cell as PickerCell;
        AlertDialog _dialog;
        AListView _listView;
        PickerAdapter _adapter;
        Context _context;
        string _valueTextCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.PickerCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public PickerCellView(Context context, Cell cell) : base(context, cell)
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
            if (e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
                e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
                e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
                e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName) {
                UpdateSelectedItems(true);
            }
            if (e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName){
                if (_PickerCell.UseAutoValueText){
                    UpdateSelectedItems(true);
                }
                else{
                    base.UpdateValueText();
                }
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateSelectedItems();
        }

        /// <summary>
        /// Updates the selected items.
        /// </summary>
        /// <param name="force">If set to <c>true</c> force.</param>
        public void UpdateSelectedItems(bool force = false)
        {
            if (!_PickerCell.UseAutoValueText){
                return;
            }


            if (force || string.IsNullOrEmpty(_valueTextCache)) {
                _valueTextCache = _PickerCell.GetSelectedItemsText();
            }

            vValueLabel.Text = _valueTextCache;
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _dialog?.Dispose();
                _dialog = null;
                _listView?.Dispose();
                _listView = null;
                _adapter?.Dispose();
                _adapter = null;
                _context = null;
            }
            base.Dispose(disposing);
        }

        internal void ShowDialog()
        {
            CreateDialog();
        }

        void CreateDialog()
        {
            _listView = new AListView(_context);
            _listView.Focusable = false;
            _listView.DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
            _listView.SetDrawSelectorOnTop(true);
            _listView.ChoiceMode = _PickerCell.MaxSelectedNumber == 1 ? Android.Widget.ChoiceMode.Single : Android.Widget.ChoiceMode.Multiple;
            _adapter = new PickerAdapter(_context, _PickerCell, _listView);
            _listView.OnItemClickListener = _adapter;
            _listView.Adapter = _adapter;

            _adapter.CloseAction = () =>
            {
                _dialog.GetButton((int)DialogButtonType.Positive).PerformClick();
            };

            if (_dialog == null) {
                using (var builder = new AlertDialog.Builder(_context)) {
                    builder.SetTitle(_PickerCell.PageTitle);
                    builder.SetView(_listView);

                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) =>
                    {
                        ClearFocus();
                    });
                    builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) =>
                    {
                        _adapter.DoneSelect();
                        UpdateSelectedItems(true);
                        _PickerCell.InvokeCommand();
                        ClearFocus();
                    });


                    _dialog = builder.Create();
                }



                _dialog.SetCanceledOnTouchOutside(true);
                _dialog.SetOnDismissListener(this);
                _dialog.SetOnShowListener(this);
                _dialog.Show();
            }
        }

        /// <summary>
        /// Ons the show.
        /// </summary>
        /// <param name="dialog">Dialog.</param>
        public void OnShow(IDialogInterface dialog)
        {
            _adapter.RestoreSelect();
        }

        /// <summary>
        /// Ons the dismiss.
        /// </summary>
        /// <param name="dialog">Dialog.</param>
        public void OnDismiss(IDialogInterface dialog)
        {
            _dialog.SetOnShowListener(null);
            _dialog.SetOnDismissListener(null);
            _dialog.Dispose();
            _dialog = null;
            _adapter?.Dispose();
            _adapter = null;
            _listView.Dispose();
            _listView = null;
            this.Selected = false;
        }
    }
}
