using Foundation;
using UIKit;

namespace Sample.iOS
{
    [Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

            AiForms.Effects.iOS.Effects.Init();
            AiForms.Renderers.iOS.SettingsViewInit.Init();
            Xamarin.Forms.Svg.iOS.SvgImage.Init();
			this.LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
