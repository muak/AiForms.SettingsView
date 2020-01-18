using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AiForms.Renderers;
using Reactive.Bindings;
using Xamarin.Forms;
using System.Linq;

namespace Sample.ViewModels
{
    public class RowManipulationTemplateViewModel
    {
        public ObservableCollection<SettingsGroup> Settings { get; set; }
        public ReactiveCommand<string> ManipulateCommand { get; } = new ReactiveCommand<string>();

        public RowManipulationTemplateViewModel()
        {
            Settings = new ObservableCollection<SettingsGroup>();
            Settings.Add(new SettingsGroup(
                new List<SettingsSectionItem> { new SettingsSectionItem { Text = "abc" } }
                ) { HeaderText = "1st", FooterText = "1stFooter" });

            ManipulateCommand.Subscribe(p => {
                switch(p)
                {
                    case "AddFirst":
                        Settings[0].Insert(0, CreateItem());
                        break;
                    case "AddLast":
                        Settings[0].Add(CreateItem());
                        break;
                    case "Add2nd":
                        Settings[0].Insert(1, CreateItem());
                        break;
                    case "DelFirst":
                        Settings[0].RemoveAt(0);
                        break;
                    case "DelLast":
                        Settings[0].Remove(Settings[0].Last());
                        break;
                    case "Del2nd":
                        Settings[0].RemoveAt(1);
                        break;
                    case "Replace1":
                        Settings[0][0] = CreateItem();
                        break;
                    case "AddSecFirst":
                        Settings.Insert(0, CreateSection());
                        break;
                    case "AddSecLast":
                        Settings.Add(CreateSection());
                        break;
                    case "AddSec2nd":
                        Settings.Insert(1, CreateSection());
                        break;
                    case "DelSecFirst":
                        Settings.RemoveAt(0);
                        break;
                    case "DelSecLast":
                        Settings.Remove(Settings.Last());
                        break;
                    case "DelSec2nd":
                        Settings.RemoveAt(1);
                        break;
                    case "ReplaceSec1":
                        Settings[0] = CreateSection();
                        break;
                    case "ShowHide1st":
                        Settings[0].IsVisible.Value = !Settings[0].IsVisible.Value;
                        break;              

                }
            });
        }

        SettingsSectionItem CreateItem()
        {
            return new SettingsSectionItem {
                Text = "AddText",
            };
        }

        SettingsGroup CreateSection()
        {
            return new SettingsGroup(new List<SettingsSectionItem> {
                new SettingsSectionItem{Text = "AddSectionText"}
            }) { HeaderText = "AddSectionHeader", FooterText = "AddFooterText" };
        }
    }

    public class SettingsGroup:ObservableCollection<SettingsSectionItem>
    {
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
        public ReactivePropertySlim<bool> IsVisible { get; set; } = new ReactivePropertySlim<bool>(true);

        public SettingsGroup(IList<SettingsSectionItem> list) : base(list) { }
    }

    public class SettingsSectionItem
    {
        public string Text { get; set; }
    }
}
