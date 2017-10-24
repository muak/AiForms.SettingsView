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
                if(_CommandCell.Command != null){
                    _CommandCell.Command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand(){
            if(_CommandCell.Command != null){
                _CommandCell.Command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            Execute = () => {
                if(_CommandCell.Command == null){
                    return;
                }
                if(_CommandCell.Command.CanExecute(_CommandCell.CommandParameter)){
                    _CommandCell.Command.Execute(_CommandCell.CommandParameter);
                }
            };

            _CommandCell.Command.CanExecuteChanged += Command_CanExecuteChanged;
            Command_CanExecuteChanged(_CommandCell.Command, System.EventArgs.Empty);
        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if(_CommandCell.Command.CanExecute(_CommandCell.CommandParameter)){
                UserInteractionEnabled = true;
                TitleLabel.Alpha = 1f;
                DescriptionLabel.Alpha = 1f;
                ValueLabel.Alpha = 1f;
            }
            else{
                UserInteractionEnabled = false;
                TitleLabel.Alpha = 0.6f;
                DescriptionLabel.Alpha = 0.6f;
                ValueLabel.Alpha = 0.6f;
            }
        }
    }

}
