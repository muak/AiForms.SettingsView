using System;
using Xamarin.Forms;
using AiForms.Renderers;
using Reactive.Bindings;
using static Sample.ViewModels.ViewModelBase;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModels
{
    public class ParentPropTestViewModel:ViewModelBase
    {
        public ParentPropTestViewModel()
        {
            PropertyChangeCommand.Subscribe(p=>{
                var text = (p as Label).Text;

                switch(text){
                    case nameof(SettingsView.BackgroundColor) :
                        NextColor(BackgroundColor, OuterColors);
                        break;
                    case nameof(SettingsView.SeparatorColor):
                        NextColor(SeparatorColor, AccentColors);
                        break;
                    case nameof(SettingsView.SelectedColor):
                        NextColor(SelectedColor, BackColors);
                        break;
                    case nameof(SettingsView.HeaderTextColor):
                        NextColor(HeaderTextColor, DeepTextColors);
                        break;
                    case nameof(SettingsView.HeaderBackgroundColor):
                        NextColor(HeaderBackgroundColor, BackColors);
                        break;
                    case nameof(SettingsView.FooterTextColor):
                        NextColor(FooterTextColor, DeepTextColors);
                        break;
                    case nameof(SettingsView.FooterBackgroundColor):
                        NextColor(FooterBackgroundColor, BackColors);
                        break;
                    case nameof(SettingsView.CellTitleColor):
                        NextColor(CellTitleColor, DeepTextColors);
                        break;
                    case nameof(SettingsView.CellValueTextColor):
                        NextColor(CellValueTextColor, PaleTextColors);
                        break;
                    case nameof(SettingsView.CellDescriptionColor):
                        NextColor(CellDescriptionColor, PaleTextColors);
                        break;
                    case nameof(SettingsView.CellBackgroundColor):
                        NextColor(CellBackgroundColor, CellBackColors);
                        break;
                    case nameof(SettingsView.CellAccentColor):
                        NextColor(CellAccentColor, AccentColors);
                        break;
                    case nameof(SettingsView.CellHintTextColor):
                        NextColor(CellHintTextColor, AccentColors);
                        break;
                    case nameof(SettingsView.ShowSectionTopBottomBorder):
                        ToggleBool(ShowSectionTopBottomBorder);
                        break;
                    case nameof(SettingsView.UseDescriptionAsValue):
                        ToggleBool(UseDescriptionAsValue);
                        break;
                    case nameof(SettingsView.HasUnevenRows):
                        ToggleBool(HasUnevenRows);
                        break;
                    case nameof(SettingsView.HeaderPadding):
                        ChangeThickness(HeaderPadding);
                        break;
                    case nameof(SettingsView.FooterPadding):
                        ChangeThickness(FooterPadding);
                        break;
                    case nameof(SettingsView.HeaderFontSize):
                        ChangeFontSize(HeaderFontSize);
                        break;
                    case nameof(SettingsView.FooterFontSize):
                        ChangeFontSize(FooterFontSize);
                        break;
                    case nameof(SettingsView.CellTitleFontSize):
                        ChangeFontSize(CellTitleFontSize);
                        break;
                    case nameof(SettingsView.CellValueTextFontSize):
                        ChangeFontSize(CellValueTextFontSize);
                        break;
                    case nameof(SettingsView.CellDescriptionFontSize):
                        ChangeFontSize(CellDescriptionFontSize);
                        break;
                    case nameof(SettingsView.CellHintFontSize):
                        ChangeFontSize(CellHintFontSize);
                        break;
                    case nameof(SettingsView.HeaderHeight):
                        ChangeHeight(HeaderHeight);
                        break;
                    case nameof(SettingsView.HeaderTextVerticalAlign):
                        ChangeAlign(HeaderTextVerticalAlign);
                        break;   
                    case nameof(SettingsView.CellIconSize):
                        ChangeSize(CellIconSize);
                        break;  
                    case nameof(SettingsView.CellIconRadius):
                        ChangeHeight(CellIconRadius);
                        break;  
                    case nameof(SettingsView.RowHeight):
                        ChangeRowHeight(RowHeight);
                        break;  

                    default:
                        break;
                }
            });
        }

        void NextColor(ReactiveProperty<Color> current,Color[] colors){
            var idx = colors.IndexOf(current.Value);
            if (idx == colors.Length - 1)
            {
                current.Value =  colors[0];
                return;
            }

            current.Value = colors[idx + 1];
        }

        void ToggleBool(ReactiveProperty<bool> current){
            current.Value = !current.Value;
        }

        void ChangeFontSize(ReactiveProperty<double> current)
        {
            if(current.Value > 30){
                current.Value = 0;
            }
            else{
                current.Value += 1.0d;
            }
        }

        void ChangeThickness(ReactiveProperty<Thickness> current)
        {
            var t = current.Value.Top;
            var b = current.Value.Bottom;
            var l = current.Value.Left;
            var r = current.Value.Right;

            var list = new List<double>{
                l,t,r,b
            };

            if(t + b + l + r < 80d){
                var idx = list.IndexOf(list.Min());
                list[idx] += 5d;

                current.Value = new Thickness(list[0], list[1], list[2], list[3]);
            }
            else{
                current.Value = new Thickness(0, 0, 0, 0);
            }
        }

        void ChangeHeight(ReactiveProperty<double> current)
        {
            if(current.Value > 80){
                current.Value = 0;
            }
            else{
                current.Value += 2;
            }
        }

        void ChangeRowHeight(ReactiveProperty<int> current)
        {
            if (current.Value > 150)
            {
                current.Value = 0;
            }
            else
            {
                current.Value += 5;
            }
        }

        void ChangeAlign(ReactiveProperty<LayoutAlignment> current)
        {
            if(current.Value == LayoutAlignment.Start){
                current.Value = LayoutAlignment.Center;
                return;
            }

            if(current.Value == LayoutAlignment.Center){
                current.Value = LayoutAlignment.End;
                return;
            }

            if(current.Value == LayoutAlignment.End){
                current.Value = LayoutAlignment.Fill;
                return;
            }

            current.Value = LayoutAlignment.Start;

        }

        void ChangeSize(ReactiveProperty<Size> current)
        {
            var size = current.Value;

            if(size.Width + size.Height > 200){
                current.Value = new Size(0, 0);
                return;
            }

            if(size.Width < 100){
                size.Width += 10;
                current.Value = new Size(size.Width, 10);
                return;
            }

            size.Height += 10;

            current.Value = new Size(size.Width, size.Height);


        }
    }
}
