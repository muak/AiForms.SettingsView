using System;
using Reactive.Bindings;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
namespace Sample.ViewModels
{
    public class DataTemplateTestViewModel:ViewModelBase
    {

        public ReactiveCommand AddCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand RepCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ClrCommand { get; set; } = new ReactiveCommand();

        public ReactiveCommand DoCommand { get; } = new ReactiveCommand();
        public ObservableCollection<Person> ItemsSource { get; set; }

        public DataTemplateTestViewModel(IPageDialogService pageDlg)
        {
            ItemsSource = new ObservableCollection<Person>(
                new List<Person>
                {
                    new Person{Name="ABC",Days="1,2,3,4"},
                    new Person{Name="DEF",Days="5,6,7,8"},
                }
            );

            DoCommand.Subscribe(async _=>{
                await pageDlg.DisplayAlertAsync("","Command","OK");
            });

            AddCommand.Subscribe(_=>{
                ItemsSource.Add(new Person { Name = "Add", Days = "9,9,9,9" });
            });

            DelCommand.Subscribe(_=>{
                ItemsSource.Remove(ItemsSource.Last());
            });

            RepCommand.Subscribe(_=>{
                ItemsSource[0] = new Person { Name = "Rep", Days = "1,1,1,1" };
            });

            ClrCommand.Subscribe(_=>{
                ItemsSource.Clear();
            });
        }

        public class Person
        {
            public string Name { get; set; }
            public string Days { get; set; }
        }
    }
}
