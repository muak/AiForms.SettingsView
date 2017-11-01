using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Services;

namespace Sample.ViewModels
{
    public class ButtonCellTestViewModel:ViewModelBase
    {
        public ReactiveProperty<TextAlignment> TitleAlignment { get; } = new ReactiveProperty<TextAlignment>();
        public ReactiveProperty<object> CommandParameter { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<bool> CanExecute { get; } = new ReactiveProperty<bool>();

        public ReactiveProperty<ReactiveCommand> Command { get; set; } = new ReactiveProperty<ReactiveCommand>();

        static ReactiveCommand[] Commands = { null, null };
        static object[] Parameters = { null, "Def", "Xzy" };
        static bool[] CanExecutes = { true, false };
        static TextAlignment[] TitleAlignments = { TextAlignment.Start, TextAlignment.Center, TextAlignment.End };

        public ButtonCellTestViewModel(IPageDialogService pageDialog)
        {
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
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);

            var text = (obj as Label).Text;

            switch (text)
            {
                case nameof(CanExecute):
                    NextVal(CanExecute, CanExecutes);
                    break;
                case nameof(Command):
                    NextVal(Command, Commands);
                    break;
                case nameof(CommandParameter):
                    NextVal(CommandParameter, Parameters);
                    break;
                case nameof(TitleAlignment):
                    NextVal(TitleAlignment, TitleAlignments);
                    break;
            }
        }
    }
}
