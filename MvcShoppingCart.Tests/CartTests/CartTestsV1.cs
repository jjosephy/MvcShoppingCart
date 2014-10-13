
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
using MvcShoppingCart.Tests.Extensions;
using MvcShoppingCart.Tests.Owin;
using Newtonsoft.Json;
using Owin;


namespace MvcShoppingCart.Tests
{
    [TestClass]
    public class CartTestsV1
    {
        [TestMethod]
        public async Task CartTest_TestAddItemCartV1()
        {
            var item = new CartItemV1
            {
                Description = "Test Description",
                Id = Guid.NewGuid(),
                Quantity = 4
            };

            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                value: item);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var cartItem = JsonConvert.DeserializeObject<CartItemV1>(result);

            Assert.IsTrue(cartItem.Id == item.Id, "Item Id's do not match");
        }

        [TestMethod]
        public async Task CartTest_TestAddInvalidAuthHeaderCartV1()
        {
            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
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
            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var errorItem = JsonConvert.DeserializeObject<ErrorDetailsV1>(result);

            Assert.IsTrue(
                errorItem.ErrorCode.Equals(ErrorCodes.InvalidCartItem),
                "Error Codes do not match");
        }

        [TestMethod]
        public async Task CartTest_TestAddThenGetCartV1()
        {
            var itemId = Guid.NewGuid();
            var item = new CartItemV1
            {
                Description = "Test Description",
                Id = itemId,
                Quantity = 4
            };

            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                value: item);

            response = await TestHost.Server.CreateRequestAsync<string>(HttpMethod.Get);
            var result = await response.Content.ReadAsStringAsync();
        }

        [TestMethod]
        public async Task CartTest_TestDeleteAllItemsFromCart()
        {
            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                value: new CartItemV1
                {
                    Description = "Test Description",
                    Id = Guid.NewGuid(),
                    Quantity = 4
                });

            // Delete all cart items to ensure there are no items in cart
            response = await TestHost.Server.CreateRequestAsync<string>(
                HttpMethod.Delete,
                uri: "/api/cart");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, "Status Codes do not match");
        }

        [TestMethod]
        public async Task CartTest_TestDeleteSingleItemFromCart()
        {
            var itemId = Guid.NewGuid();
            var response = await TestHost.Server.CreateRequestAsync<CartItemV1>(
                HttpMethod.Post,
                value: new CartItemV1
                {
                    Description = "Test Description",
                    Id = itemId,
                    Quantity = 4
                });

            // Delete all cart items to ensure there are no items in cart
            response = await TestHost.Server.CreateRequestAsync<string>(
                HttpMethod.Delete,
                uri: "api/cart/" + itemId.ToString());
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, "Status Codes do not match");
        }

        [TestMethod]
        public async Task CartTest_TestNoItemsInCartExceptionV1()
        {
            // Delete all cart items to ensure there are no items in cart
            var response = await TestHost.Server.CreateRequestAsync<string>(HttpMethod.Delete);
            var result = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound, "Status Codes do not match");
        }

        //TODO: need a test for passing no auth or version header
    }
}
