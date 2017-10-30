using System;
using Reactive.Bindings;
using Xamarin.Forms;
using AiForms.Renderers;
using Xamarin.Forms.Internals;
using Prism.Services;
using System.Reactive.Linq;

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
    public class LabelCellTestViewModel : ViewModelBase
    {
        public ReactiveProperty<TimeSpan> Time { get; } = new ReactiveProperty<TimeSpan>();
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
        public ReactiveProperty<object> CommandParameter { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<bool> CanExecute { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> KeepSelected { get; } = new ReactiveProperty<bool>();

        public ReactiveCommand<int> NumberSelectedCommand { get; set; } = new ReactiveCommand<int>();
        public ReactiveProperty<ReactiveCommand> Command { get; set; } = new ReactiveProperty<ReactiveCommand>();


        static int[] Numbers = { 0, 5, 10, 15 };
        static int[] MaxNumbers = { 0, 10, 15, 1 };
        static int[] MinNumbers = { 0, 1, 15, 10 };
        static string[] PickerTitles = { "Hoge", "LongTitleFugaFugaFugaFuga", "" };
        static TimeSpan[] Times = { new TimeSpan(0, 0, 0), new TimeSpan(12, 30, 0), new TimeSpan(23, 20, 15), new TimeSpan(47, 55, 0) };
        static string[] TimeFormats = { "t", "hh:mm", "H:m" };
        static DateTime[] Dates = { new DateTime(2017, 1, 1), new DateTime(2015, 1, 1), new DateTime(2017, 6, 10) };
        static DateTime[] MinDates = { new DateTime(2016, 1, 1), new DateTime(2017, 4, 1), new DateTime(2017, 10, 10), new DateTime(2017, 12, 15) };
        static DateTime[] MaxDates = { new DateTime(2025, 12, 31), new DateTime(2017, 5, 15), new DateTime(2017, 10, 10), new DateTime(2017, 6, 15) };
        static string[] DateFormats = { "d", "yyyy/M/d (ddd)", "ddd MMM d yyyy" };
        static string[] TodayTexts = { "Today", "今日", "" };
        static ReactiveCommand[] Commands = { null, null };
        static object[] Parameters = { null, "Def", "Xzy" };
        static bool[] CanExecutes = { true, false };

        public LabelCellTestViewModel(IPageDialogService pageDialog)
        {
            BackgroundColor.Value = Color.White;
            PickerTitle.Value = "Hoge";

            TimeFormat.Value = "t";
            Time.Value = new TimeSpan(12, 0, 0);

            Date.Value = Dates[0];
            MinDate.Value = MinDates[0];
            MaxDate.Value = MaxDates[0];
            DateFormat.Value = DateFormats[0];
            TodayText.Value = TodayTexts[0];

            CanExecute.Value = CanExecutes[0];

            Commands[0] = CanExecute.ToReactiveCommand();
            Commands[1] = CanExecute.ToReactiveCommand();

            CommandParameter.Value = Parameters[0];
            Command.Value = Commands[0];

            Commands[0].Subscribe(async p =>
            {
                await pageDialog.DisplayAlertAsync("Command1", p?.ToString(), "OK");
            });

            Commands[1].Subscribe(async p =>
            {
                await pageDialog.DisplayAlertAsync("Command2", p?.ToString(), "OK");
            });
            NumberSelectedCommand.Subscribe(async p =>
            {
                await pageDialog.DisplayAlertAsync("", p.ToString(), "OK");
            });
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch (text)
            {
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
                case nameof(Time):
                    NextVal(Time, Times);
                    break;
                case nameof(TimeFormat):
                    NextVal(TimeFormat, TimeFormats);
                    break;
                case nameof(Date):
                    NextVal(Date, Dates);
                    break;
                case nameof(DateFormat):
                    NextVal(DateFormat, DateFormats);
                    break;
                case "MinMaxDateChange":
                    NextVal(MinDate, MinDates);
                    NextVal(MaxDate, MaxDates);
                    break;
                case nameof(TodayText):
                    NextVal(TodayText, TodayTexts);
                    break;
                case nameof(CanExecute):
                    NextVal(CanExecute, CanExecutes);
                    break;
                case nameof(Command):
                    NextVal(Command, Commands);
                    break;
                case nameof(CommandParameter):
                    NextVal(CommandParameter, Parameters);
                    break;
                case nameof(KeepSelected):
                    NextVal(KeepSelected, CanExecutes);
                    break;
            }

        }


    }
}
