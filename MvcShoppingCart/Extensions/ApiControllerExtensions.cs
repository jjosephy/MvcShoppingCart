using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MvcShoppingCart.Extensions
{
    public static class ApiControllerExtensions
    {
        public static ICartRequestContext CreateCartContext(this ApiController controller)
        {
            return new CartRequestContext
            {
                UserToken = controller.Request.Headers.GetAuthorizationHeader(),
                Version = controller.Request.Headers.GetVersionRequestHeader()
            };
        }
    }
}