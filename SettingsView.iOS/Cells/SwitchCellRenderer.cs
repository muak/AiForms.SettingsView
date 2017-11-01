using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using AiSwitchCellRenderer = AiForms.Renderers.iOS.SwitchCellRenderer;
using AiSwitchCell = AiForms.Renderers.SwitchCell;

[assembly: ExportRenderer(typeof(AiSwitchCell), typeof(AiSwitchCellRenderer))]
namespace AiForms.Renderers.iOS
{
    public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

    public class SwitchCellView : CellBaseView
    {
        public AiSwitchCell _SwitchCell => Cell as AiSwitchCell;
        UISwitch _switch;

        public SwitchCellView(Cell formsCell) : base(formsCell)
        {

            _switch = new UISwitch();
            _switch.ValueChanged += _switch_ValueChanged;

            this.AccessoryView = _switch;
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == AiSwitchCell.AccentColorProperty.PropertyName) {
                UpdateAccentColor();
            }
            if (e.PropertyName == AiSwitchCell.OnProperty.PropertyName) {
                UpdateOn();
            }
        }

        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName) {
                UpdateAccentColor();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateAccentColor();
            UpdateOn();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _switch.ValueChanged -= _switch_ValueChanged;
                AccessoryView = null;
                _switch?.Dispose();
                _switch = null;
            }
            base.Dispose(disposing);
        }

        void _switch_ValueChanged(object sender, EventArgs e)
        {
            _SwitchCell.On = _switch.On;
        }

        void UpdateOn()
        {
            if (_switch.On != _SwitchCell.On) {
                _switch.On = _SwitchCell.On;
            }
        }

        void UpdateAccentColor()
        {
            if (_SwitchCell.AccentColor != Xamarin.Forms.Color.Default) {
                _switch.OnTintColor = _SwitchCell.AccentColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default) {
                _switch.OnTintColor = CellParent.CellAccentColor.ToUIColor();
            }
        }

    }
}
