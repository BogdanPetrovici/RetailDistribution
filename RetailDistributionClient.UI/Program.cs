using MvvmDialogs;
using RetailDistribution.Client.UI.ViewModels;
using RetailDistribution.Client.UI.Views;
using SimpleInjector;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RetailDistribution.Client.UI
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			var container = Bootstrap();

			// Any additional other configuration, e.g. of your desired MVVM toolkit.

			RunApplication(container);
		}

		private static Container Bootstrap()
		{
			// Create the container as usual.
			var container = new Container();

			//Initialize service for opening dialog boxes
			container.Register<IDialogService>(() => { return new DialogService(); }, Lifestyle.Singleton);

			//Initialize HttpClient for calls to webapi
			container.Register<HttpClient>(() =>
			{
				var client = new HttpClient();
				// Update port # in the following line.
				client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["localServerAddress"]);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));
				return client;
			}, Lifestyle.Singleton);


			container.Register<MainViewModel>();
			container.Register<Main>();

			container.Verify();

			return container;
		}

		private static void RunApplication(Container container)
		{
			try
			{
				var app = new App();
				//app.InitializeComponent();
				var mainWindow = container.GetInstance<Main>();
				app.Run(mainWindow);
			}
			catch (Exception ex)
			{
				//Log the exception and exit
			}
		}
	}
}
