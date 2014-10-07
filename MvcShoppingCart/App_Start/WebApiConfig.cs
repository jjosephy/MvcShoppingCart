using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MvcShoppingCart
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
//            config.Filters.Add(new AuthorizeAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
