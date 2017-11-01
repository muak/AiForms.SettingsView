using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Sample.Views
{
    public partial class OnTableViewTest : ContentPage
    {
        public OnTableViewTest()
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            DisplayAlert("","Tapped","OK");
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            DisplayAlert("", "Completed", "OK");
        }
    }
}
