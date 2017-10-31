using System;
using Xamarin.Forms;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModels
{
    public class PickerCellTestViewModel:ViewModelBase
    {
        public ReactiveProperty<Color> OwnAccentColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<string> PageTitle { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> DisplayMember { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> DisplayMember2 { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> MaxSelectedNumber { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> KeepSelected { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> SelectedItemsOrderKey { get; } = new ReactiveProperty<string>();

        public ObservableCollection<Person> ItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<int> ItemsSource2 { get; } = new ObservableCollection<int>();

        public ObservableCollection<Person> SelectedItems { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<int> SelectedItems2 { get; } = new ObservableCollection<int>();

        static string[] PageTitles = { "", "Select value", "LongTitleTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd" };
        static string[] DisplayMembers = { "Name", "Age",""};
        static int[] MaxSelectedNumbers = { 0, 1, 3 };
        static bool[] bools = { false, true };
       
 
        public PickerCellTestViewModel()
        {
            for (var i = 0; i < 30; i++)
            {
                ItemsSource.Add(new Person()
                {
                    Name = $"Name{i}",
                    Age = 30-i
                });
                ItemsSource2.Add(i);
            }

            DisplayMember.Value = "Name";
            DisplayMember2.Value = "";

            OwnAccentColor.Value = AccentColor;
            PageTitle.Value = PageTitles[0];
            MaxSelectedNumber.Value = MaxSelectedNumbers[0];
            KeepSelected.Value = bools[0];
            SelectedItemsOrderKey.Value = DisplayMembers[0];
            ChangeSelectedItems();
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch (text)
            {
                case nameof(OwnAccentColor):
                    NextVal(OwnAccentColor, AccentColors);
                    break;
                case nameof(PageTitle):
                    NextVal(PageTitle, PageTitles);
                    break;
                case nameof(DisplayMember):
                    NextVal(DisplayMember, DisplayMembers);
                    break;
                case nameof(MaxSelectedNumber):
                    NextVal(MaxSelectedNumber, MaxSelectedNumbers);
                    break;
                case nameof(KeepSelected):
                    NextVal(KeepSelected, bools);
                    break;
                case nameof(SelectedItems):
                    ChangeSelectedItems();
                    break;
                case nameof(SelectedItemsOrderKey):
                    NextVal(SelectedItemsOrderKey, DisplayMembers);
                    break;
            }
        }

        void ChangeSelectedItems(){
            if(SelectedItems.Count == 0){
                SelectedItems = new ObservableCollection<Person>(ItemsSource.Where(x => x.Age > 1 && x.Age < 6));
            }
            else{
                SelectedItems = new ObservableCollection<Person>();
            }

            RaisePropertyChanged(nameof(SelectedItems));
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
