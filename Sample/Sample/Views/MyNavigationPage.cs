using System;
using Xamarin.Forms;
using AiForms.Effects;
namespace Sample.Views
{
    public class MyNavigationPage:NavigationPage
    {
        public MyNavigationPage()
        {
            BarTextColor = Color.FromHex("#CC9900");
            //BarBackgroundColor = Color.White;
            AlterColor.SetOn(this,true);
            AlterColor.SetAccent(this,Color.FromHex("#CC9900"));
        }
    }
}
