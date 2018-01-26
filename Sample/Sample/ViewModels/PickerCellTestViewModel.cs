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
        public ReactiveProperty<string> SubDisplayMember { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> DisplayMember2 { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> MaxSelectedNumber { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> KeepSelected { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> SelectedItemsOrderKey { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> UseNaturalSort { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> UseAutoValueText { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> UsePickToClose { get; } = new ReactiveProperty<bool>();

        public ObservableCollection<Person> ItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<int> ItemsSource2 { get; } = new ObservableCollection<int>();

        public ObservableCollection<Person> SelectedItems { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<int> SelectedItems2 { get; } = new ObservableCollection<int>();

        public ReactiveCommand SelectedCommand { get; } = new ReactiveCommand();

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
                //ItemsSource2.Add(i);
            }

            DisplayMember.Value = "Name";
            DisplayMember2.Value = "";
            SubDisplayMember.Value = DisplayMembers[1];
            UseAutoValueText.Value = true;
            UsePickToClose.Value = false;

            OwnAccentColor.Value = AccentColor;
            PageTitle.Value = PageTitles[0];
            MaxSelectedNumber.Value = MaxSelectedNumbers[0];
            KeepSelected.Value = bools[0];
            SelectedItemsOrderKey.Value = DisplayMembers[0];
            ChangeSelectedItems();

            SelectedCommand.Subscribe(obj=>{
                var list = obj as ObservableCollection<Person>;
                System.Diagnostics.Debug.WriteLine(list);
            });
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
                    NextVal(SubDisplayMember,DisplayMembers);
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
                case nameof(UseNaturalSort):
                    NextVal(UseNaturalSort, bools);
                    break;
                case nameof(UseAutoValueText):
                    NextVal(UseAutoValueText, bools);
                    break;
                case nameof(UsePickToClose):
                    NextVal(UsePickToClose, bools);
                    break;
                case "AddItem":
                    ItemsSource2.Add(new Random().Next(1,30));
                    break;
                case "RemoveItem":
                    if(ItemsSource2.Count > 0){
                        ItemsSource2.RemoveAt(0);
                    }
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
