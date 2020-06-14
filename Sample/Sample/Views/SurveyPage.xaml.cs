using System;
using System.Collections.Generic;
using AiForms.Renderers;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class SurveyPage : ContentPage
    {
        public SurveyPage()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            settings.Root[0].Add(new LabelCell { Title = "A" });
        }
    }
}
