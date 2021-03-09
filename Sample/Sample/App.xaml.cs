using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Sample.Views;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace Sample
{
    public partial class App : PrismApplication
	{
		protected override void OnInitialized()
		{
			InitializeComponent();

            Xamarin.Forms.Svg.SvgImageSource.RegisterAssembly();

            NavigationService.NavigateAsync("MyNavigationPage/MainPage");
            //MainPage = new AppShell();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MyNavigationPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ContentPage>();

            containerRegistry.RegisterForNavigation<ButtonCellTest>();
            containerRegistry.RegisterForNavigation<CollectionChangedTest>();
            containerRegistry.RegisterForNavigation<CustomCellTest>();
            containerRegistry.RegisterForNavigation<CustomFontTest>();
            containerRegistry.RegisterForNavigation<DataTemplateTest>();
            containerRegistry.RegisterForNavigation<DummyPage>();
            containerRegistry.RegisterForNavigation<EntryCellTest>();
            containerRegistry.RegisterForNavigation<FormsCellTest>();
            containerRegistry.RegisterForNavigation<GlobalDataTemplate>();
            containerRegistry.RegisterForNavigation<LabelCellTest>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<OnTableViewTest>();
            containerRegistry.RegisterForNavigation<ParentPropTest>();
            containerRegistry.RegisterForNavigation<PickerCellTest>();
            containerRegistry.RegisterForNavigation<RadioCellTemplateTest>();
            containerRegistry.RegisterForNavigation<RadioCellTest>();

            containerRegistry.RegisterForNavigation<ReorderTest>();
            containerRegistry.RegisterForNavigation<RowManipulation>();
            containerRegistry.RegisterForNavigation<RowManipulationTemplate>();
            containerRegistry.RegisterForNavigation<SettingsViewPage>();
            //containerRegistry.RegisterForNavigation<ShellTestPage>();
            containerRegistry.RegisterForNavigation<SurveyPage>();
            containerRegistry.RegisterForNavigation<SwitchCellTest>();

            //this.GetType().GetTypeInfo().Assembly
            //.DefinedTypes
            //.Where(t => t.Namespace?.EndsWith(".Views", System.StringComparison.Ordinal) ?? false)
            //.ForEach(t => {
            //    containerRegistry.RegisterForNavigation(t.AsType(), t.Name);
            //});
        }
	}
}

