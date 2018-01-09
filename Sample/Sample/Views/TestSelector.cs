using System;
using Xamarin.Forms;
using AiForms.Renderers;
using static Sample.ViewModels.DataTemplateTestViewModel;

namespace Sample.Views
{
    public class TestSelector : DataTemplateSelector
    {
        public DataTemplate TemplateA { get; set; }
        public DataTemplate TemplateB { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var hoge = item as Person;
            return hoge.Name.Contains("A") ? TemplateA : TemplateB;
        }
    }
}
