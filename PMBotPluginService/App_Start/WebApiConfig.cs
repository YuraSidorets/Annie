using System.Web.Http;

namespace PMBotPluginService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // New code
            config.EnableCors();

            config.Routes.MapHttpRoute(
            name: "PluginRoute",
            routeTemplate: "api/{controller}/{action}"
        );
            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}
