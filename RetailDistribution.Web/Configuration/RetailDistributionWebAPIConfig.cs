using System;
using System.Web.Http;

namespace HelloWebAPI.Configuration
{
	public static class RetailDistributionWebAPIConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "RetailDistributionApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "DistrictApi",
				routeTemplate: "api/district/{district}",
				defaults: new { id = RouteParameter.Optional }
			);

			// configure json formatter
			//JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;
			//jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
								.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
																						"text/html",
																						StringComparison.InvariantCultureIgnoreCase,
																						true,
																						"application/json"));
		}
	}
}