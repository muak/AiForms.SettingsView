using System;
using Reactive.Bindings;
using Xamarin.Forms;
using AiForms.Renderers;
using Xamarin.Forms.Internals;

namespace Sample.ViewModels
{
    /// <summary>
    /// 各Cellの各項目の
    /// テキスト変更
    /// 色・フォントサイズ・背景色 変更
    /// アイコンソース・サイズ・角丸 変更
    /// それからそれらの親項目を変更してみて子が優先されるかを確認する
    /// 
    /// </summary>
    public class LabelCellTestViewModel:ViewModelBase
    {
        public ReactiveProperty<DateTime> Time { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<string> TimeFormat { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> PickerTitle { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> Number { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> MaxNum { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> MinNum { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<DateTime> Date { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<string> DateFormat { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<DateTime> MaxDate { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<DateTime> MinDate { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<string> TodayText { get; } = new ReactiveProperty<string>();

        public ReactiveCommand<int> NumberSelectedCommand { get; set; } = new ReactiveCommand<int>();

        static int[] Numbers = { 0, 5, 10 ,15};
        static int[] MaxNumbers = { 0,10, 15 ,  1};
        static int[] MinNumbers = { 0, 1, 15 , 10 };
        static string[] PickerTitles = { "Hoge", "LongTitleFugaFugaFugaFuga", "" };

        public LabelCellTestViewModel()
        {
            BackgroundColor.Value = Color.White;
            PickerTitle.Value = "Hoge";

        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch(text){
                case nameof(NumberPickerCell.Number):
                    NextVal(Number, Numbers);
                    break;
                case "MaxMinChange":
                    NextVal(MaxNum, MaxNumbers);
                    NextVal(MinNum, MinNumbers);                       

                    break;
                case nameof(NumberPickerCell.PickerTitle):
                    NextVal(PickerTitle, PickerTitles);
                    break;
            }

        }

        void NextVal<T>(ReactiveProperty<T> current, T[] array)
        {
            var idx = array.IndexOf(current.Value);
            if (idx == array.Length - 1)
            {
                current.Value = array[0];
                return;
            }

            current.Value = array[idx + 1];
        }
    }
}
