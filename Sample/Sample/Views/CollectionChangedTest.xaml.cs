using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AiForms.Renderers;
using System.Linq;

namespace Sample.Views
{
    public partial class CollectionChangedTest : ContentPage
    {
        public CollectionChangedTest()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var text = (sender as Button).Text;
            switch(text){
                case "AddCell":
                    AddCell();
                    break;
                case "DeleteCell":
                    DeleteCell();
                    break;
                case "AddSection":
                    AddSection();
                    break;
                case "DeleteSection":
                    DeleteSection();
                    break;
                case "SectionVisible":
                    SectionVisible();
                    break;
                case "ReplaceCell":
                    ReplaceCell();
                    break;
                case "ReplaceSection":
                    ReplaceSection();
                    break;
                default:
                    break;
            }
        }


        int AddCellCount = 0;
        void AddCell()
        {
            var cell = new LabelCell
            {
                Title = "AddCell",
                ValueText = "addcell",
                Description = "add cell",
                HintText = "hint"
            };

            switch(AddCellCount){
                case 0:
                    settings.Root[0].Insert(0, cell);
                    break;
                case 1:
                    settings.Root[0].Add(cell);
                    break;
                case 2:
                    settings.Root[0].Insert(settings.Root[0].Count / 2, cell);
                    break;
                case 3:
                    settings.Root[1].Insert(0,cell);
                    break;
                case 4:
                    settings.Root[1].Add(cell);
                    break;
                case 5:
                    settings.Root[1].Insert(settings.Root[1].Count / 2, cell);
                    break;

            }

            AddCellCount++;
            if(AddCellCount > 5){
                AddCellCount = 0;
            }
        }

        void ReplaceCell()
        {
            settings.Root[0][0] = settings.Root[0][2];
        }

        int DeleteCellCount = 0;
        void DeleteCell()
        {
            switch (DeleteCellCount)
            {
                case 0:
                    settings.Root[0].RemoveAt(0);
                    break;
                case 1:
                    settings.Root[0].Remove(settings.Root[0].Last());
                    break;
                case 2:
                    settings.Root[0].RemoveAt(settings.Root[0].Count / 2);
                    break;
                case 3:
                    settings.Root[1].RemoveAt(0);
                    break;
                case 4:
                    settings.Root[1].Remove(settings.Root[1].Last());
                    break;
                case 5:
                    settings.Root[1].RemoveAt(settings.Root[1].Count / 2);
                    break;

            }

            DeleteCellCount++;
            if (DeleteCellCount > 5)
            {
                DeleteCellCount = 0;
            }
        }

        void AddSection()
        {
            var section = new AiForms.Renderers.Section("AddedSection");
            section.Add(
               new LabelCell
                {
                    Title = "AddCell",
                    ValueText = "addcell",
                    Description = "add cell in new section",
                    HintText = "hint"
                }
            );
            settings.Root.Add(section);
        }

        void DeleteSection()
        {
            settings.Root.Remove(settings.Root.Last());
        }

        void ReplaceSection()
        {
            settings.Root[0] = settings.Root[1];
        }

        int SectionVisibleCount = 0;
        void SectionVisible()
        {
            settings.Root[SectionVisibleCount].IsVisible = !settings.Root[SectionVisibleCount].IsVisible;

            SectionVisibleCount++;

            if(SectionVisibleCount >= settings.Root.Count){
                SectionVisibleCount = 0;
            }
        }
    }
}
