using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace Sample.ViewModels
{
    class PickerSurveyViewModel
    {
        public ObservableCollection<Person> MasterItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> MasterItemsSourceSelectedItems { get; } = new ObservableCollection<Person>();

        public ObservableCollection<Person> SlaveItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> SlaveItemsSourceSelectedItems { get; } = new ObservableCollection<Person>();

        string[] type = { "letters", "number" };

        string[] listLetters = { "a", "b", "c", "d", "e" };
        string[] listNumbers = { "1", "2", "3", "4", "5" };

        public PickerSurveyViewModel()
        {
            foreach (var item in type)
            {
                MasterItemsSource.Add(new Person()
                {
                    Name = item,
                    Age = 1
                });
            }

        }

        ICommand selectMasterCommand;
        public ICommand SelectMasterCommand =>
            selectMasterCommand ?? (selectMasterCommand = new Command(async () => await ExecuteSelectMasterCommand()));

        async Task ExecuteSelectMasterCommand()
        {
            try
            {

                if (MasterItemsSourceSelectedItems.Count == 0)
                    return;
 
                SlaveItemsSource.Clear();
                SlaveItemsSourceSelectedItems.Clear();
                

                await Task.Delay(100);

                switch (MasterItemsSourceSelectedItems[0].Name)
                {
                    case "letters":

                        foreach (var item in listLetters)
                        {
                            SlaveItemsSource.Add(new Person()
                            {
                                Name = item,
                                Age = 1
                            });
                        }


                        break;
                    case "number":
                        foreach (var item in listNumbers)
                        {
                            SlaveItemsSource.Add(new Person()
                            {
                                Name = item,
                                Age = 1
                            });
                        }


                        break;
                   
                    default:
                        break;
                }


                SlaveItemsSourceSelectedItems.Add(SlaveItemsSource[0]);

            }
            catch (Exception ex)
            {
             
            }
            finally
            {
               
            }
        }
    }

    //public class Person
    //{
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //}


}
