using System.Linq;
using System.Reflection;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Sample.Views;

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
            //MainPage = new AppShell();
		}

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ContentPage>();

            this.GetType().GetTypeInfo().Assembly
            .DefinedTypes
            .Where(t => t.Namespace?.EndsWith(".Views", System.StringComparison.Ordinal) ?? false)
            .ForEach(t => {
                containerRegistry.RegisterForNavigation(t.AsType(), t.Name);
            });
        }
	}
}

