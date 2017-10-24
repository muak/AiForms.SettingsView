using AiForms.Renderers;
using AiForms.Renderers.iOS;
using Xamarin.Forms;
using System;
using UIKit;
using System.Windows.Input;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class CommandCellRenderer:CellBaseRenderer<CommandCellView>{}

    public class CommandCellView : LabelCellView
    {
        public Action Execute { get; set; }
        CommandCell _CommandCell => Cell as CommandCell;
        ICommand _command;

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
                if(_command != null){
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand(){
            if(_command != null){
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = _CommandCell.Command;

            if (_command != null) {
                _command.CanExecuteChanged += Command_CanExecuteChanged;
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () => {
                if(_command == null){
                    return;
                }
                if(_command.CanExecute(_CommandCell.CommandParameter)){
                    _command.Execute(_CommandCell.CommandParameter);
                }
            };

        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if(_command.CanExecute(_CommandCell.CommandParameter)){
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
