using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MvcShoppingCart.Extensions;

namespace MvcShoppingCart.Extensions
{
    public class CartRequestContext : ICartRequestContext
    {
        public string UserToken
        {
            get;
            set;
        }

        public uint Version
        {
            get;
            set;
        }

        public static ICartRequestContext CreateContext(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Create a good exception")
                });
            }
            
            return new CartRequestContext
            {
                UserToken = request.Headers.GetAuthorizationHeader(),
                Version = request.Headers.GetVersionRequestHeader()
            };

        }
    }
}