using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Xamarin.Forms;
using System;
using UIKit;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class CommandCellRenderer:CellBaseRenderer<CommandCellView>{}

    public class CommandCellView : LabelCellView
    {
        public Action Execute { get; set; }
        CommandCell _CommandCell => Cell as CommandCell;

        public CommandCellView(Cell formsCell) : base(formsCell)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            SelectionStyle = UITableViewCellSelectionStyle.Default;
            SetRightMarginZero();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if(e.PropertyName == CommandCell.CommandProperty.PropertyName ||
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName){
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
            if(disposing){
                Execute = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand(){
            Execute = () => {
                if(_CommandCell.Command == null){
                    return;
                }
                if(_CommandCell.Command.CanExecute(_CommandCell.CommandParameter)){
                    _CommandCell.Command.Execute(_CommandCell.CommandParameter);
                }
            };
        }
    }

}
