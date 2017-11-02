using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Unity;
using Sample.Views;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace Sample
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();

            Xamarin.Forms.Svg.SvgImageSource.RegisterAssembly();

			NavigationService.NavigateAsync("MyNavigationPage/MainPage");
		}

		protected override void RegisterTypes()
		{
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<ContentPage>();
			//Container.RegisterTypeForNavigation<MainPage>();
			//Container.RegisterTypeForNavigation<SettingsViewPage>();
            //Container.RegisterTypeForNavigation<ParentPropTest>();
            //Container.RegisterTypeForNavigation<DefaultPropTest>();
            //Container.RegisterTypeForNavigation<CollectionChangedTest>();
            //Container.RegisterTypeForNavigation<LabelCellTest>();

            this.GetType().GetTypeInfo().Assembly
            .DefinedTypes
            .Where(t => t.Namespace?.EndsWith(".Views", System.StringComparison.Ordinal) ?? false)
            .ForEach(t => {
                Container.RegisterTypeForNavigation(t.AsType(), t.Name);
            });
		}
	}
}

