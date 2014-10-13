using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcShoppingCart.Contracts;
using MvcShoppingCart.Tests.Owin;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace MvcShoppingCart.Tests.CartTests
{
    [TestClass]
    public class CartTestsV2
    {
        [TestMethod]
        public async Task CartTest_TestAddItemCartV2()
        {
            var id = Guid.NewGuid();
            var item = new CartItemV2
            {
                Description = "Test Description",
                Id = id,
                Quantity = 4,
                Color = "Blue"
            };
            

            var response = await TestHost.Server.CreateRequestAsync<CartItemV2>(
                HttpMethod.Post,
                value: item, 
                version: 2);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, "Status Codes do not match");
            var result = await response.Content.ReadAsStringAsync();
            var cartItem = JsonConvert.DeserializeObject<CartItemV1>(result);

            Assert.IsTrue(cartItem.Id == item.Id, "Item Id's do not match");
        }
    }
}
