using System;
using System.Collections.Generic;
using AiForms.Renderers;
using Xamarin.Forms;
using System.Linq;

namespace Sample.Views
{
    public partial class RowManipulation : ContentPage
    {
        public RowManipulation()
        {
            InitializeComponent();
        }

        void AddFirstClicked(object sender, System.EventArgs e)
        {
            section.Insert(0, CreateCell());
        }

        void AddLastClicked(object sender, System.EventArgs e)
        {
            section.Add(CreateCell());
        }

        void Add2ndClicked(object sender, System.EventArgs e)
        {
            section.Insert(1, CreateCell());
        }

        void DelFirstClicked(object sender, System.EventArgs e)
        {
            section.RemoveAt(0);
        }

        void DelLastClicked(object sender, System.EventArgs e)
        {
            section.Remove(section.Last());
        }

        void Del2ndClicked(object sender, System.EventArgs e)
        {
            section.RemoveAt(1);
        }

        void Replace3To1Clicked(object sender, System.EventArgs e)
        {
            section[0] = section[2];
        }


        Cell CreateCell()
        {
            return new LabelCell {
                Title = "AddCell",
                ValueText = "addcell",
                Description = "add cell",
                HintText = "hint"
            };
        }
    }
}
