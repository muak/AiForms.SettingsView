using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using Prism.Navigation;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace Sample.ViewModels
{
    public class PickerSurveyViewModel:BindableBase,INavigatingAware
    {
        public ObservableCollection<Person> MasterItemsSource { get; set; }
        public ObservableCollection<Person> MasterItemsSourceSelectedItems { get; set; } = new ObservableCollection<Person>();

        string[] type = { "letters", "number" };

        string[] listLetters = { "a", "b", "c", "d", "e" };
        string[] listNumbers = { "1", "2", "3", "4", "5" };

        public PickerSurveyViewModel()
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            var list = new List<Person>{
                new Person {Name = "A",Age = 20},
                new Person {Name = "B",Age = 30}
            };

            MasterItemsSource = new ObservableCollection<Person>(list);
            MasterItemsSourceSelectedItems.Add(list[0]);


            RaisePropertyChanged(nameof(MasterItemsSource));


        }
    }
}
