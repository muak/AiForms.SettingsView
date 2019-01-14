using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AiForms.Renderers;
using System.Collections;

namespace Sample.ViewModels
{
    public class GlobalDataTemplateViewModel
    {
        public ObservableCollection<SectionItem> ItemsSource { get; } = new ObservableCollection<SectionItem>();
        public GlobalDataTemplateViewModel()
        {
            ItemsSource.Add(new SectionItem(
                new List<SettingItem>{
                    new SettingItem { Title = "TitleA", Name = "AAA"},
                    new SettingItem { Title = "TitleB", Name = "BBB"},
                }
            ) { 
                SectionTitle = "SectionA" 
            });

            ItemsSource.Add(new SectionItem(
                new List<SettingItem>{
                    new SettingItem { Title = "TitleC", Name = "CCC"},
                    new SettingItem { Title = "TitleD", Name = "DDD"},
                }
            ) {
                SectionTitle = "SectionB"
            });

        }

        public class SettingItem
        {
            public string Title { get; set; }
            public string Name { get; set; }
        }

        public class SectionItem:ObservableCollection<SettingItem>
        {
            public SectionItem(IEnumerable<SettingItem> list):base(list){}
            public string SectionTitle { get; set; }
        }
    }
}
