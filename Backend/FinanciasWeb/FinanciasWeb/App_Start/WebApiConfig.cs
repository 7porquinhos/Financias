using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinanciasWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET, POST, PUT, PATCH, DELETE, OPTIONS"); 
            config.EnableCors(cors);
            */

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //     name: "ControllerOnly",
            //     routeTemplate: "api/{controller}"
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
