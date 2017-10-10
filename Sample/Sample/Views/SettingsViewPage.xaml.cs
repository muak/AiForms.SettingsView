using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;
using AiForms.Renderers;
using Xamarin.Forms.Svg;

namespace Sample.Views
{
	public partial class SettingsViewPage : ContentPage
	{
        
		public SettingsViewPage()
		{
			InitializeComponent();

            for (var i = 1; i <= 20; i++){
                var cell = new LabelCell {
                    Title = $"Cell{i}",
                    ValueText = $"Value{i}",
                    Description = $"Description{i}",
                    IconSource = SvgImageSource.FromSvg($"icon{i}.svg", 50, 50),
                    IconSize = new Size(25, 25)
                };
                settings.Root[0].Add(cell);
            }
		}

		void AddCell(object sender, System.EventArgs e)
		{
            //settings.Root[0].Add(new TextCell{Text="added"});
            //settings.CellTitleColor = Color.Red;
            SettingsView.ClearCache();
		}

		void AddSection(object sender, System.EventArgs e)
		{
			var section = new AiForms.Renderers.Section("AddedSection");
			section.Add(
				new TextCell{Text="added"}
			);
			settings.Root.Add(section);
		}

        void DelCell(object sender, System.EventArgs e)
        {
            settings.Root[0].Remove(settings.Root[0].Last());
        }

        void DelSec(object sender, System.EventArgs e)
        {
            settings.Root.Remove(settings.Root.Last());
        }
	}
}
