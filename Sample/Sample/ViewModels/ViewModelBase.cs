using System;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class ViewModelBase:BindableBase
    {
        public ReactiveProperty<Color> BackgroundColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Color> SeparatorColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Color> SelectedColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Thickness> HeaderPadding { get; } = new ReactiveProperty<Thickness>();
        public ReactiveProperty<Color> HeaderTextColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<double> HeaderFontSize { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<LayoutAlignment> HeaderTextVerticalAlign { get; } = new ReactiveProperty<LayoutAlignment>();
        public ReactiveProperty<Color> HeaderBackgroundColor { get; }= new ReactiveProperty<Color>();
        public ReactiveProperty<double> HeaderHeight { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<Color> FooterTextColor { get; }= new ReactiveProperty<Color>(); 
        public ReactiveProperty<double> FooterFontSize { get; }= new ReactiveProperty<double>();
        public ReactiveProperty<Color> FooterBackgroundColor { get; }= new ReactiveProperty<Color>();
        public ReactiveProperty<Thickness> FooterPadding { get; }= new ReactiveProperty<Thickness>();
        public ReactiveProperty<int> RowHeight { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> HasUnevenRows { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<Color> CellTitleColor { get; }= new ReactiveProperty<Color>();
        public ReactiveProperty<double> CellTitleFontSize { get; } =new ReactiveProperty<double>();
        public ReactiveProperty<Color> CellValueTextColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<double> CellValueTextFontSize { get; }= new ReactiveProperty<double>();
        public ReactiveProperty<Color> CellDescriptionColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<double> CellDescriptionFontSize { get; } =new ReactiveProperty<double>();
        public ReactiveProperty<Color> CellBackgroundColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Size> CellIconSize { get; } = new ReactiveProperty<Size>();
        public ReactiveProperty<double> CellIconRadius { get; } =new ReactiveProperty<double>();
        public ReactiveProperty<Color> CellAccentColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Color> CellHintTextColor { get; }= new ReactiveProperty<Color>();
        public ReactiveProperty<double> CellHintFontSize { get; }= new ReactiveProperty<double>();
        public ReactiveProperty<bool> UseDescriptionAsValue { get; }= new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ShowSectionTopBottomBorder { get; }= new ReactiveProperty<bool>();

        public ReactiveCommand PropertyChangeCommand { get; } = new ReactiveCommand();



        public static Color OuterColor = Color.DeepPink;
        public static Color AccentColor = Color.FromHex("#1133CC");
        public static Color BackColor   = Color.FromHex("#ADB8CC");
        public static Color CellBackColor = Color.White;
        public static Color DeepTextColor  = Color.FromHex("#3367CC");
        public static Color PaleTextColor  = Color.FromHex("#A3B1CC");
		
        public static Color OuterColor2 = Color.Blue;
        public static Color AccentColor2 = Color.FromHex("#FF407F");
        public static Color BackColor2 = Color.FromHex("#CCB8BE");
        public static Color CellBackColor2 = Color.FromHex("#B8CCAD");
        public static Color DeepTextColor2 = Color.FromHex("#EE508F");
        public static Color PaleTextColor2 = Color.FromHex("#CCA3B0");

        public static Color[] OuterColors = {
            OuterColor,OuterColor2,Color.Transparent
        };
        public static Color[] AccentColors = {
            AccentColor,AccentColor2,Color.Transparent
        };
        public static Color[] BackColors = {
            BackColor,BackColor2,Color.Transparent
        };
        public static Color[] CellBackColors = {
            CellBackColor,CellBackColor2,Color.Transparent
        };
        public static Color[] DeepTextColors = {
            DeepTextColor,DeepTextColor2,Color.Transparent
        };
        public static Color[] PaleTextColors = {
            PaleTextColor,PaleTextColor2,Color.Transparent
        };

        public ViewModelBase()
        {
            BackgroundColor.Value = OuterColor;
            SeparatorColor.Value = AccentColor;
            SelectedColor.Value = BackColor;
            HeaderTextColor.Value = DeepTextColor;
            HeaderBackgroundColor.Value = Color.Transparent;
            FooterTextColor.Value = DeepTextColor;
            FooterBackgroundColor.Value = Color.Transparent;
            CellTitleColor.Value = DeepTextColor;
            CellValueTextColor.Value = PaleTextColor;
            CellDescriptionColor.Value = PaleTextColor;
            CellBackgroundColor.Value = CellBackColor;
            CellAccentColor.Value = AccentColor;
            CellHintTextColor.Value = AccentColor;

            HeaderFontSize.Value = 10;
            HeaderHeight.Value = 16;
            FooterFontSize.Value = 10;
            CellTitleFontSize.Value = 14;
            CellValueTextFontSize.Value = 12;
            CellDescriptionFontSize.Value = 10;
            CellIconSize.Value = new Size(30, 30);
            CellIconRadius.Value = 4;
            CellHintFontSize.Value = 9;

            HasUnevenRows.Value = false;
            UseDescriptionAsValue.Value = false;
            ShowSectionTopBottomBorder.Value = true;


            //RowHeight.Value = 150;
        }
    }
}
