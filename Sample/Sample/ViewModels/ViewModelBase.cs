using System;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class ViewModelBase:BindableBase
    {
        public ReactiveProperty<Color> BackgroundColor { get; } 
        public ReactiveProperty<Color> SeparatorColor { get; } 
        public ReactiveProperty<Thickness> HeaderPadding { get; } 
        public ReactiveProperty<Color> HeaderTextColor { get; } 
        public ReactiveProperty<double> HeaderFontSize { get; } 
        public ReactiveProperty<LayoutAlignment> HeaderTextVerticalAlign { get; } 
        public ReactiveProperty<Color> HeaderBackgroundColor { get; }
        public ReactiveProperty<double> HeaderHeight { get; } 
        public ReactiveProperty<Color> FooterTextColor { get; } 
        public ReactiveProperty<double> FooterFontSize { get; } 
        public ReactiveProperty<Color> FooterBackgroundColor { get; }
        public ReactiveProperty<Thickness> FooterPadding { get; } 
        public ReactiveProperty<int> RowHeight { get; }
        public ReactiveProperty<bool> HasUnevenRows { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<Color> CellTitleColor { get; }
        public ReactiveProperty<double> CellTitleFontSize { get; } 
        public ReactiveProperty<Color> CellValueTextColor { get; } 
        public ReactiveProperty<double> CellValueTextFontSize { get; } 
        public ReactiveProperty<Color> CellDescriptionColor { get; } 
        public ReactiveProperty<double> CellDescriptionFontSize { get; } 
        public ReactiveProperty<Color> CellBackgroundColor { get; } 
        public ReactiveProperty<Size> CellIconSize { get; } 
        public ReactiveProperty<Color> CellAccentColor { get; } 
        public ReactiveProperty<Color> CellHintTextColor { get; }
        public ReactiveProperty<double> CellHintFontSize { get; } 

    }
}
