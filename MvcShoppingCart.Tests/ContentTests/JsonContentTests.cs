using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcShoppingCart.HttpContentFormatter;
using MvcShoppingCart.Contracts;
using Newtonsoft.Json;

namespace MvcShoppingCart.Tests
{
    [TestClass]
    public class JsonContentTests
    {
        [TestMethod]
        public async Task JsonContent_TestContentFormatterV1()
        {
            var cartItem = new CartItemV1
            {
                Description = "test item",
                Id = Guid.NewGuid(),
                Quantity = 2
            };
            
            var stringContent = await new JsonContent<CartItemV1>(cartItem).ReadAsStringAsync();
            var formattedCartItem = JsonConvert.DeserializeObject<CartItemV1>(stringContent);
            Assert.AreEqual(cartItem.Quantity, formattedCartItem.Quantity, "Quanties of Cart Items are not equal");
            Assert.AreEqual(cartItem.Id, formattedCartItem.Id, "Ids of Cart Items are not equal");
            Assert.AreEqual(cartItem.Description, formattedCartItem.Description, "Descriptions of Cart Items are not equal");
        }
    }
}
