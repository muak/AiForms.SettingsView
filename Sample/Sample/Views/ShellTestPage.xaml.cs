using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sample.ViewModels;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class ShellTestPage : ContentPage
    {
        public ObservableCollection<Person> ItemsSource { get; set; } = new ObservableCollection<Person>();

        public ShellTestPage()
        {
            InitializeComponent();

            for (var i = 0; i < 30; i++)
            {
                ItemsSource.Add(new Person() {
                    Name = $"Name{i}",
                    Age = 30 - i
                });
            }

            

            this.BindingContext = this;
        }
    }
}
