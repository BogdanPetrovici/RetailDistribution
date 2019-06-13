using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

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
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"\d*" }
            );

            config.Routes.MapHttpRoute(
                name: "RetailDistributionApiGeneric",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
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