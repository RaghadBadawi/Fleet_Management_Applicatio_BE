using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;

namespace TProject2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            var cors = new EnableCorsAttribute("*", "*", "*"); 
            config.EnableCors(cors);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

    }
}
