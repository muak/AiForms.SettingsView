using System;
using System.Collections.Generic;
using Reactive.Bindings;
namespace Sample.ViewModels
{
    public class CustomCellTestViewModel
    {
        public List<int> ItemsSource { get; }
        public ReactiveCommand<double> ChangedCommand { get; } = new ReactiveCommand<double>();

        public CustomCellTestViewModel()
        {
            ItemsSource = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            ChangedCommand.Subscribe(val => {
                System.Diagnostics.Debug.WriteLine($"Value: {val}");
            });
        }
    }
}
