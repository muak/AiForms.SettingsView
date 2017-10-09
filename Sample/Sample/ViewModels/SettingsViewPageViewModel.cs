using System;
using Reactive.Bindings;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Svg;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sample.ViewModels
{
    public class SettingsViewPageViewModel : BindableBase
    {
        public ReactiveProperty<string> Cell1 { get; } = new ReactiveProperty<string>("ABC");
        public ReactiveProperty<string> Cell2 { get; } = new ReactiveProperty<string>("DEF");
        public ReactiveProperty<string> Cell3 { get; } = new ReactiveProperty<string>("ViewCell");

        public ReactiveProperty<string> Title1 { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Title2 { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<bool> Sec1Visible { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> Sec2Visible { get; } = new ReactiveProperty<bool>(true);

        public ReactiveProperty<bool> HasUneven { get; } = new ReactiveProperty<bool>(true);

        public ReactiveCommand ToggleVisible { get; set; } = new ReactiveCommand();
        public ReactiveCommand ToggleRowHeight { get; set; } = new ReactiveCommand();

        public ReactiveCommand NextPageCommand { get; set; } = new ReactiveCommand();

        public ReactiveProperty<ImageSource> Image { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveProperty<string> Description { get; } = new ReactiveProperty<string>();

        public ReactiveCommand<int> CellCommand { get; } = new ReactiveCommand<int>();
        public ReactiveProperty<int> CellCommandParameter { get; } = new ReactiveProperty<int>(1);

        public List<int> IntList { get; } = new List<int>();
        public List<int> SelectedInt { get; } = new List<int>();

        public ObservableCollection<Person> PickerItems { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> SelectedItems { get; }

        public ReactiveProperty<bool> SwitchOn { get; } = new ReactiveProperty<bool>();

        public ReactiveCommand GeneralCommand { get; set; } = new ReactiveCommand();

        public SettingsViewPageViewModel(INavigationService navigationService, IPageDialogService pageDlg)
        {
            Title1.Value = "aaaaaafsdfsdfsdfあｆｄさｆｓｄｆｓｄふぁｓだｆｓｄｆさｆｄｓｆｓふぁｓｆｓだふぁｆ";
            Title2.Value = "Sec2";

            ToggleVisible.Subscribe(_ => {
                if (Sec1Visible.Value) {
                    Sec1Visible.Value = false;
                    return;
                }
                if (Sec2Visible.Value) {
                    Sec2Visible.Value = false;
                    return;
                }

                Sec1Visible.Value = true;
                Sec2Visible.Value = true;

            });

            ToggleRowHeight.Subscribe(_ => {
                HasUneven.Value = !HasUneven.Value;
            });

            //Image.Value = SvgImageSource.FromSvg("alert-circled.svg",50,50);
            Description.Value = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaEND";

            NextPageCommand.Subscribe(async _ => {
                await navigationService.NavigateAsync("MainPage");
            });



            for (var i = 0; i < 20; i++) {
                PickerItems.Add(new Person() {
                    Name = $"Name{i}",
                    Age = i
                });
                IntList.Add(i);
            }

            SelectedItems = new ObservableCollection<Person>(PickerItems.Where(x => x.Age == 1));


            CellCommand.Subscribe(async p => {
                //await pageDlg.DisplayAlertAsync("", $"{p}", "OK");
                await navigationService.NavigateAsync("MainPage");
                //var afssa = SelectedItems;
                SwitchOn.Value = false;
            });

            GeneralCommand.Subscribe(_=>{
                var fasf = _;
            });
        }

        public class Person{
            public string Name { get; set; }
            public int Age { get; set; }
        }

    }
}
