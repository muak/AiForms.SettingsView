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

            settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == SettingsView.VisibleContentHeightProperty.PropertyName)
            {
                var height = settings.VisibleContentHeight;
                settings.HeightRequest = height;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
    }
}
