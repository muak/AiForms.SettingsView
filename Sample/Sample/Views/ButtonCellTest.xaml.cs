using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Sample.Views
{
    public partial class ButtonCellTest : ContentPage
    {
        public ButtonCellTest()
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            DisplayAlert("","Tapped","OK");
        }
    }
}
