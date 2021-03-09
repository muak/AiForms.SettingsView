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
        public ObservableCollection<Hoge> ItemsSource { get; set; }
        public ReactivePropertySlim<string> Text { get; } = new ReactivePropertySlim<string>();
        public ReactiveCommand ChangeCommand { get; } = new ReactiveCommand();

        public SurveyPageViewModel()
        {
            ItemsSource = new ObservableCollection<Hoge>(new List<Hoge> {
                new Hoge{Name="A",Value=1},
                new Hoge{Name="B",Value=2},
                new Hoge{Name="C",Value=3}
            });

            Text.Value = "テキスト";

            var toggle = true;
            ChangeCommand.Subscribe(_ => {
                if (toggle)
                {
                    Text.Value = "テキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキスト";
                }
                else
                {
                    Text.Value = "テキスト";
                }
                toggle = !toggle;
            });

        }        

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
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
