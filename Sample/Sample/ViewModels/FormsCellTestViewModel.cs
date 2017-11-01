using System;
using Xamarin.Forms;
namespace Sample.ViewModels
{
    public class FormsCellTestViewModel:ViewModelBase
    {
        public FormsCellTestViewModel()
        {
            IconSource.Value = IconSources[0];
            TitleColor.Value = DeepTextColors[0];
            DescriptionColor.Value = PaleTextColors[0];
           
            RowHeight.Value = 60; //Androidでは60以上じゃないとFormsCellが正しく表示されない
        }
    }
}
