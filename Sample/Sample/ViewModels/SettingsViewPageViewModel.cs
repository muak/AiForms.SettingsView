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
        



        public ReactiveCommand ToProfileCommand { get; set; } = new ReactiveCommand();

        public ReactiveProperty<ImageSource> Image { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveProperty<string> Description { get; } = new ReactiveProperty<string>();

        public ReactiveCommand<int> CellCommand { get; } = new ReactiveCommand<int>();
        public ReactiveProperty<int> CellCommandParameter { get; } = new ReactiveProperty<int>(1);

        public List<int> IntList { get; } = new List<int>();
        public List<int> SelectedInt { get; } = new List<int>();

        public ObservableCollection<Person> ItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> SelectedItems { get; } = new ObservableCollection<Person>();

        public ReactiveProperty<bool> SwitchOn { get; } = new ReactiveProperty<bool>();

        public ReactiveCommand GeneralCommand { get; set; } = new ReactiveCommand();

        string[] languages = { "Java", "C#", "JavaScript", "PHP", "Perl", "C++",  "Swift", "Kotlin", "Python", "Ruby", "Scala", "F#" };

        public SettingsViewPageViewModel(INavigationService navigationService, IPageDialogService pageDlg)
        {
            


            ToProfileCommand.Subscribe(async _ => {
                await navigationService.NavigateAsync("ContentPage");
            });



            foreach(var item in languages){
                ItemsSource.Add(new Person() {
                    Name = item,
                    Age = 1
                });
            }

            SelectedItems.Add(ItemsSource[1]);
            SelectedItems.Add(ItemsSource[2]);
            SelectedItems.Add(ItemsSource[3]);




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
