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
            settings.Root[0].Insert(0, CreateCell());
        }

        void AddLastClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].Add(CreateCell());
        }

        void Add2ndClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].Insert(1, CreateCell());
        }

        void DelFirstClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].RemoveAt(0);
        }

        void DelLastClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].Remove(section.Last());
        }

        void Del2ndClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].RemoveAt(1);
        }

        void Replace1Clicked(object sender, System.EventArgs e)
        {
            settings.Root[0][0] = CreateCell();
        }

        void AddSecFirstClicked(object sender, System.EventArgs e)
        {
            settings.Root.Insert(0, CreateSection());
        }

        void AddSecLastClicked(object sender, System.EventArgs e)
        {
            settings.Root.Add(CreateSection());
        }

        void AddSec2ndClicked(object sender, System.EventArgs e)
        {
            settings.Root.Insert(1, CreateSection());
        }

        void DelSecFirstClicked(object sender, System.EventArgs e)
        {
            settings.Root.RemoveAt(0);
        }

        void DelSecLastClicked(object sender, System.EventArgs e)
        {
            settings.Root.Remove(settings.Root.Last());
        }

        void DelSec2ndClicked(object sender, System.EventArgs e)
        {
            settings.Root.RemoveAt(1);
        }

        void ReplaceSec1Clicked(object sender, System.EventArgs e)
        {
            settings.Root[0] = CreateSection();
        }

        void ShowHide1stClicked(object sender, System.EventArgs e)
        {
            settings.Root[0].IsVisible = !settings.Root[0].IsVisible;
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

        Section CreateSection()
        {
            var sec = new AiForms.Renderers.Section() {
                Title = "Additional Section",
                FooterText = "Footer"
            };
            sec.Add(
               new LabelCell {
                   Title = "AddCell",
                   ValueText = "addcell",
                   Description = "add cell in new section",
                   HintText = "hint"
               }
            );
            return sec;
        }
    }
}
