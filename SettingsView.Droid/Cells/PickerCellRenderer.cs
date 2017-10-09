using System;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using AListView = Android.Widget.ListView;
using System.Collections.Generic;
using Xamarin.Forms.Platform.Android;
using System.Collections;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class PickerCellRenderer:CellBaseRenderer<PickerCellView>{}

    public class PickerCellView : LabelCellView,IDialogInterfaceOnShowListener,IDialogInterfaceOnDismissListener
    {
        PickerCell _PickerCell => Cell as PickerCell;
        AlertDialog _dialog;
        AListView _listView;
        PickerAdapter _adapter;
        Context _context;


        public PickerCellView(Context context, Cell cell) : base(context, cell)
        {
            _context = context;
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName)
            {

            }
            else if (e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName)
            {

            }
            else if (e.PropertyName == PickerCell.KeepSelectedUntilBackProperty.PropertyName)
            {

            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
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
            _adapter = new PickerAdapter(_context,_PickerCell,_listView);
            _listView.OnItemClickListener = _adapter;
            _listView.Adapter = _adapter;


            if (_dialog == null)
            {
                using (var builder = new AlertDialog.Builder(_context))
                {
                    builder.SetTitle(_PickerCell.PageTitle);
                    builder.SetView(_listView);

                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) => {
                        ClearFocus();
                    });
                    builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) => {
                        _adapter.DoneSelect();
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

        public void OnShow(IDialogInterface dialog){
            _adapter.RestoreSelect();
        }

        public void OnDismiss(IDialogInterface dialog){
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
