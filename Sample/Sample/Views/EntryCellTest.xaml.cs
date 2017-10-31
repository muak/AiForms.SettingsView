using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Sample.Views
{
    public partial class EntryCellTest : ContentPage
    {
        public EntryCellTest()
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            //DisplayAler11t("","Tapped","OK");
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            DisplayAlert("", "Completed", "OK");
        }
    }
}
