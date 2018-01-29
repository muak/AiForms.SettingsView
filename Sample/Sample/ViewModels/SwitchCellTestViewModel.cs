using System;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class SwitchCellTestViewModel:ViewModelBase
    {
        public ReactiveProperty<Color> OwnAccentColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<bool> On { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> Checked { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsVisible { get; } = new ReactiveProperty<bool>(true);

        static bool[] bools = { false, true };
        public SwitchCellTestViewModel()
        {
            BackgroundColor.Value = Color.White;
            OwnAccentColor.Value = AccentColor;
            On.Value = false;
            Checked.Value = false;

        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch (text)
            {
                case nameof(OwnAccentColor):
                    NextVal(OwnAccentColor, AccentColors);
                    break;
                case nameof(On):
                    NextVal(On, bools);
                    break;
                case nameof(Checked):
                    NextVal(Checked, bools);
                    break;
                case nameof(IsVisible):
                    NextVal(IsVisible, bools);
                    break;
            }
        }
    }
}
