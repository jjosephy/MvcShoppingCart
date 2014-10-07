
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MvcShoppingCart.Contracts;
using MvcShoppingCart.Exceptions;
using MvcShoppingCart.Extensions;
using MvcShoppingCart.HttpContentFormatter;
using MvcShoppingCart.Models;
using Newtonsoft.Json;

namespace MvcShoppingCart.Controllers
{
    public class CartController : ApiController
    {
        /// <summary>
        /// In memory store of cart items
        /// </summary>
        private static Dictionary<string, Dictionary<Guid, CartItemModel>> cartItems = 
            new Dictionary<string, Dictionary<Guid, CartItemModel>>();

        /// <summary>
        /// This will get all of the items in your cart
        /// </summary>
        /// <returns>A list of all items in the cart</returns>
        public Task<HttpResponseMessage> Get()
        {
            var content =  new string[]
            {
                 "Hello",
                 "World"
            };

            return Task.FromResult(new HttpResponseMessage
            {
                Content = new JsonContent<string[]>(content)
            });
        }

        /// <summary>
        /// Handler to Add an Item to the Cart
        /// </summary>
        /// <returns>Returns the item that was added.</returns>
        public async Task<HttpResponseMessage> Post()
        {
            var authToken = this.Request.Headers.GetAuthorizationHeader();
            var version = this.Request.Headers.GetVersionRequestHeader();
            CartItemBase cartRequestBody = null;
            var returnMessage = new HttpResponseMessage();

            switch (version)
            {
                case 1:
                    cartRequestBody = JsonConvert.DeserializeObject<CartItemV1>(
                        await this.Request.Content.ReadAsStringAsync());
                    break;
                case 2:
                default:
                    throw CartException.InvalidVersionHeader();
            }

            if (cartRequestBody == null)
            {
                throw CartException.InvalidCartItemException();
            }

            var userItems = default(Dictionary<Guid, CartItemModel>);
            if (!cartItems.TryGetValue(authToken, out userItems))
            {
                userItems = new Dictionary<Guid,CartItemModel>();
                cartItems.Add(authToken, userItems);
            }

            if (userItems.ContainsKey(cartRequestBody.Id))
            {
                throw CartException.ItemAlreadyExistsInCartException();
            }
            else
            {
                switch (version)
                {
                    case 1:
                        var item = cartRequestBody as CartItemV1;
                        userItems.Add(item.Id, item.ToModel());
                        break;
                }
            }

            switch (version)
            {
                case 1:
                    returnMessage = new HttpResponseMessage
                    {
                        Content = new JsonContent<CartItemV1>(cartRequestBody as CartItemV1)
                    };
                    break;
            }

            return returnMessage;
        }
    }
}
