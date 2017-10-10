using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace Sample.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		public ReactiveCommand<string> GoToPage { get; set; } = new ReactiveCommand<string>();
        public ReactiveCommand<string> GoToTest { get; set; } = new ReactiveCommand<string>();

		public MainPageViewModel(INavigationService navigationService)
		{
            GoToPage.Subscribe(async p=>{
                await navigationService.NavigateAsync(p + "Page");
            });
            GoToTest.Subscribe(async p=>{
                await navigationService.NavigateAsync(p);
            });
           
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}
	}
}

