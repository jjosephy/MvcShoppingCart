
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
    /// Class that serves as an in memory host server as well as provides client hooks to 
    /// fire requests through the server.
    /// </summary>
    public sealed class TestHost : IDisposable
    {
        const string CartApiServer = "http://www.TemporaryTestServer.com";
        const string CartApi = "/api/cart";
        const string ApiVersion = "x-api-version";
        const string Authorization = "authorization";
        const string TestUser = "TestUser";

        readonly TestServer server;
        readonly HttpClient client;

        static volatile TestHost host;
        static readonly object syncRoot = new object();

        private TestHost()
        {
            this.server = TestServer.Create<OwinTestConfiguration>();
            this.client = new HttpClient(this.server.Handler)
            {
                BaseAddress = new Uri(CartApiServer)
            };
        }

        /// <summary>
        /// Server instance
        /// </summary>
        public static TestHost Server
        {
            get
            {
                if (host == null)
                {
                    lock (syncRoot)
                    {
                        if (host == null)
                        {
                            host = new TestHost();
                        }
                    }
                }

                return host;
            }
        }

        public async Task<HttpResponseMessage> CreateRequestAsync<T>(
            HttpMethod method, 
            string authToken = TestUser, 
            T value = default(T),
            string uri = CartApi,
            uint version = 1)
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

            throw new NotSupportedException("Method not supported");
        }

        public void Dispose()
        {
            if ( server != null )
            {
                server.Dispose();
            }

            if ( client != null )
            {
                client.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
