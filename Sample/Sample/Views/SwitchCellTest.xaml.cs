using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Sample.Views
{
    public partial class SwitchCellTest : ContentPage
    {
        public SwitchCellTest()
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            //Androidは発火しなくて正解
            DisplayAlert("","Tapped","OK");
        }
    }
}
