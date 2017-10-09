using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Xamarin.Forms;
using Android.Support.V7.Widget;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class CommandCellRenderer:CellBaseRenderer<CommandCellView>{}

    public class CommandCellView : LabelCellView
    {
        public System.Action Execute { get; set; }
        CommandCell _CommandCell => Cell as CommandCell;

        public CommandCellView(Context context, Cell cell) : base(context, cell)
        {
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CommandCell.CommandProperty.PropertyName ||
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName)
            {
                UpdateCommand();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateCommand();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Execute = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand()
        {
            Execute = () => {
                if (_CommandCell.Command == null)
                {
                    return;
                }
                if (_CommandCell.Command.CanExecute(_CommandCell.CommandParameter))
                {
                    _CommandCell.Command.Execute(_CommandCell.CommandParameter);
                }
            };
        }
    }
}
