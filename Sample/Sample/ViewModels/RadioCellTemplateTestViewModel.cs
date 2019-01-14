using System;
using System.Collections.Generic;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class RadioCellTemplateTestViewModel
    {
        public List<int> ValueTypes { get; set; } = new List<int>();
        public List<Hoge> RefTypes { get; set; } = new List<Hoge>();

        public ReactivePropertySlim<int> SelectedValue { get; } = new ReactivePropertySlim<int>();
        public ReactivePropertySlim<Hoge> SelectedRef { get; } = new ReactivePropertySlim<Hoge>();

        public RadioCellTemplateTestViewModel()
        {
            ValueTypes.Add(1);
            ValueTypes.Add(2);
            ValueTypes.Add(3);

            RefTypes.Add(new Hoge { Name = "A", Id = 1 });
            RefTypes.Add(new Hoge { Name = "B", Id = 2 });
            RefTypes.Add(new Hoge { Name = "C", Id = 3 });

            SelectedValue.Value = ValueTypes[1];
            SelectedRef.Value = RefTypes[1];

        }

        public class Hoge
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}
