using System;
using Reactive.Bindings;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Sample.ViewModels
{
    public class RadioCellTestViewModel:ViewModelBase
    {
        public ReactiveProperty<Color> OwnAccentColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<RadioItem> Selected { get; set; } = new ReactiveProperty<RadioItem>();
        public ReactiveProperty<RadioItem> GlobalSelected { get; set; }
        public List<RadioItem> RadioItems { get; } = new List<RadioItem>();
        public ReactivePropertySlim<bool> ToggleGlobal { get; } = new ReactivePropertySlim<bool>();


        public RadioCellTestViewModel()
        {
            RadioItems.AddRange(new List<RadioItem> {
                new RadioItem{Name = "TypeA",Value = 1},
                new RadioItem{Name = "TypeB",Value = 2},
                new RadioItem{Name = "TypeC",Value = 3},
                new RadioItem{Name = "TypeD",Value = 4},
                new RadioItem{Name = "TypeE",Value = 5},
                new RadioItem{Name = "TypeF",Value = 6},
            });

            Selected.Value = RadioItems[1];
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch (text) {
                case nameof(OwnAccentColor):
                    NextVal(OwnAccentColor, AccentColors);
                    break;
                case nameof(ToggleGlobal):
                    if (!ToggleGlobal.Value) {
                        Selected = null;
                        GlobalSelected = new ReactiveProperty<RadioItem>(RadioItems[1]);
                    }
                    else
                    {
                        GlobalSelected = null;
                        Selected = new ReactiveProperty<RadioItem>(RadioItems[1]);
                    }
                    ToggleGlobal.Value = !ToggleGlobal.Value;
                    RaisePropertyChanged(nameof(Selected));
                    RaisePropertyChanged(nameof(GlobalSelected));
                    break;
                case "ChangeValue":
                    if(!ToggleGlobal.Value)
                    {
                        NextVal(Selected, RadioItems.ToArray());
                    }
                    else
                    {
                        NextVal(GlobalSelected, RadioItems.ToArray());
                    }
                    break;
               
            }
        }

        public class RadioItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
