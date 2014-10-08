﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using MvcShoppingCart.Tests.Extensions;

namespace MvcShoppingCart.Tests.Owin
{
    /// <summary>
    /// TODO: Refactor this as a singleton
    /// Class that serves as an in memory host server as well as provides client hooks to 
    /// fire requests through the server.
    /// </summary>
    public class TestHost : IDisposable
    {
        const string CartApi = "http://testserver/api/cart";
        const string ApiVersion = "x-api-version";
        const string Authorization = "authorization";
        const string TestUser = "TestUser";

        readonly TestServer server;

        public TestHost()
        {
            server = TestServer.Create<OwinTestConfiguration>();
        }

        public async Task<HttpResponseMessage> CreateRequestAsync<T>(
            HttpMethod method, 
            string authToken = TestUser, 
            T value = default(T),
            string uri = CartApi,
            uint version = 1)
        {
            using (var client = new HttpClient(server.Handler))
            {
                client.DefaultRequestHeaders.Add(ApiVersion, version.ToString());
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("APIToken", TestUtilty.Base64Encode(authToken));

                if (method == HttpMethod.Get)
                {
                    return await client.GetAsync(uri);
                }
                else if (method == HttpMethod.Post)
                {
                    return await client.PostAsJsonAsync<T>(uri, value);
                }
                else if (method == HttpMethod.Delete)
                {
                    return await client.DeleteAsync(uri);
                }
            }

            throw new NotSupportedException("Method not supported");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (server != null)
                {
                    server.Dispose();
                }
            } 
        }
    }
}
