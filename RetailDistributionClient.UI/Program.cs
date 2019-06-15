using MvvmDialogs;
using RetailDistribution.Client.UI.ViewModels;
using RetailDistribution.Client.UI.Views;
using SimpleInjector;
using System;

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

			// Register your types, for instance:
			//container.Register<IQueryProcessor, QueryProcessor>(Lifestyle.Singleton);
			//container.Register<IUserContext, WpfUserContext>();

			container.Register<IDialogService>(() => { return new DialogService(); }, Lifestyle.Singleton);

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
