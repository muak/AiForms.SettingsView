using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Navigation;

namespace Sample.ViewModels
{
    public class SurveyPageViewModel:BindableBase, INavigatedAware
    {
        public ObservableCollection<Hoge> ItemsSource { get; set; }

        public SurveyPageViewModel()
        {
            ItemsSource = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
        }        

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            

            //RaisePropertyChanged(nameof(ItemsSource));
        }

        public class Hoge
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }

    
}
