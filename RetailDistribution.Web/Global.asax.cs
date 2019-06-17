using HelloWebAPI.Configuration;
using RetailDistribution.Data;
using RetailDistribution.Data.Repositories;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Web.Http;

namespace RetailDistribution.Web
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			// Create the container as usual.
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

			container.Register<IDistrictRepository, DistrictRepository>(Lifestyle.Scoped);
			container.Register<IVendorRepository, VendorRepository>(Lifestyle.Scoped);
			container.Register<IShopRepository, ShopRepository>(Lifestyle.Scoped);
			container.Register<IRetailDistributionContext, RetailDistributionContext>(Lifestyle.Scoped);
			container.Register<IRetailDistributionUnitOfWork, RetailDistributionUnitOfWork>(Lifestyle.Scoped);

			// This is an extension method from the integration package.
			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver =
				new SimpleInjectorWebApiDependencyResolver(container);


			GlobalConfiguration.Configure(RetailDistributionWebAPIConfig.Register);
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}