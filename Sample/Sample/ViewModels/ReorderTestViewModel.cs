using System;
using Reactive.Bindings;
using System.Collections.ObjectModel;
namespace Sample.ViewModels
{
    public class ReorderTestViewModel
    {
        public ObservableCollection<string> ItemsSource { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ItemsSource2 { get; } = new ObservableCollection<string>();

        public ReorderTestViewModel()
        {
            ItemsSource.Add("1 Abc");
            ItemsSource.Add("2 Def");
            ItemsSource.Add("3 Ghi");
            ItemsSource.Add("4 Jkl");
            ItemsSource.Add("5 Mno");

            ItemsSource2.Add("6 Pqr");
            ItemsSource2.Add("7 Stu");
            ItemsSource2.Add("8 Vxy");
        }
    }
}
