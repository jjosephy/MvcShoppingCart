
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcShoppingCart.Controllers;
using MvcShoppingCart.Contracts;
using MvcShoppingCart.Exceptions;
using MvcShoppingCart.Owin;
using MvcShoppingCart.Tests.Extensions;
using MvcShoppingCart.Tests.Owin;
using Newtonsoft.Json;
using Owin;


namespace MvcShoppingCart.Tests
{
    [TestClass]
    public class CartTestsV1
    {
        static TestHost host;
        static string authToken;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            host = new TestHost();
            authToken = TestUtilty.GenerateRandomString();
        }

        [ClassCleanup()]
        public static void MyClassCleanup() 
        {
            if (host != null)
            {
                host.Dispose();
            }
        }

        [TestMethod]
        public async Task CartTest_TestAddItemCartV1()
        {
            var item = new CartItemV1
            {
                Description = "Test Description",
                Id = Guid.NewGuid(),
                Quantity = 4
            };

            var response = await host.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                authToken,
                item);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var cartItem = JsonConvert.DeserializeObject<CartItemV1>(result);

            Assert.IsTrue(cartItem.Id == item.Id, "Item Id's do not match");
        }

        [TestMethod]
        public async Task CartTest_TestAddInvalidAuthHeaderCartV1()
        {
            var response = await host.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                string.Empty,
                null);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Forbidden, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var errorItem = JsonConvert.DeserializeObject<ErrorDetailsV1>(result);

            Assert.IsTrue(
                errorItem.ErrorCode.Equals(ErrorCodes.InvalidAuthorizationHeader),
                "Error Codes do not match");
        }

        [TestMethod]
        public async Task CartTest_TestAddInvalidItemToCartV1()
        {
            var response = await host.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post, 
                authToken,
                null);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var errorItem = JsonConvert.DeserializeObject<ErrorDetailsV1>(result);

            Assert.IsTrue(
                errorItem.ErrorCode.Equals(ErrorCodes.InvalidCartItem),
                "Error Codes do not match");
        }

        [TestMethod]
        public async Task CartTest_TestGetCartV1()
        {
            var response = await host.CreateRequestAsync<string>(HttpMethod.Get, authToken);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
