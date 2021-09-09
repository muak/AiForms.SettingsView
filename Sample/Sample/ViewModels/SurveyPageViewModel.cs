using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;

namespace Sample.ViewModels
{
    public class SurveyPageViewModel:BindableBase, INavigatedAware
    {
        public ObservableCollection<Hoge> ItemsSource1 { get; set; }
        public ObservableCollection<Hoge> ItemsSource2 { get; set; }
        public ObservableCollection<Hoge> ItemsSource3 { get; set; }
        public ObservableCollection<Hoge> ItemsSource4 { get; set; }
        public ObservableCollection<Hoge> ItemsSource5 { get; set; }
        public ObservableCollection<Hoge> ItemsSource6 { get; set; }
        public ObservableCollection<Hoge> ItemsSource7 { get; set; }
        public ObservableCollection<Hoge> ItemsSource8 { get; set; }
        public ObservableCollection<Hoge> ItemsSource9 { get; set; }
        public ObservableCollection<Hoge> ItemsSource10 { get; set; }
        public ObservableCollection<Hoge> ItemsSource11 { get; set; }
        public ObservableCollection<Hoge> ItemsSource12 { get; set; }
        public ObservableCollection<Hoge> ItemsSource13 { get; set; }
        public ObservableCollection<Hoge> ItemsSource14 { get; set; }
        public ObservableCollection<Hoge> ItemsSource15 { get; set; }
        public ObservableCollection<Hoge> ItemsSource16 { get; set; }
        public ObservableCollection<Hoge> ItemsSource17 { get; set; }
        public ObservableCollection<Hoge> ItemsSource18 { get; set; }
        public ObservableCollection<Hoge> ItemsSource19 { get; set; }
        public ObservableCollection<Hoge> ItemsSource20 { get; set; }

        public SurveyPageViewModel()
        {
            ItemsSource1 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource2 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="D",Value=1},
                new Hoge{Name="E",Value=2},
                new Hoge{Name="F",Value=3}
            });
            ItemsSource3 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="G",Value=1},
                new Hoge{Name="H",Value=2},
                new Hoge{Name="I",Value=3}
            });
            ItemsSource4 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="J",Value=1},
                new Hoge{Name="K",Value=2},
                new Hoge{Name="L",Value=3}
            });
            ItemsSource5 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="M",Value=1},
                new Hoge{Name="N",Value=2},
                new Hoge{Name="O",Value=3}
            });
            ItemsSource6 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="P",Value=1},
                new Hoge{Name="Q",Value=2},
                new Hoge{Name="R",Value=3}
            });
            ItemsSource7 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="S",Value=1},
                new Hoge{Name="T",Value=2},
                new Hoge{Name="U",Value=3}
            });
            ItemsSource8 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="V",Value=1},
                new Hoge{Name="W",Value=2},
                new Hoge{Name="X",Value=3}
            });
            ItemsSource9 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="AA",Value=1},
                new Hoge{Name="AB",Value=2},
                new Hoge{Name="AC",Value=3}
            });
            ItemsSource10 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="AD",Value=1},
                new Hoge{Name="AE",Value=2},
                new Hoge{Name="AF",Value=3}
            });
            ItemsSource11 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource12 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource13 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource14 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource15 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource16 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource17 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource18 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource19 = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });
            ItemsSource20 = new ObservableCollection<Hoge>(new List<Hoge> {
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
