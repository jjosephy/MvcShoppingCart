
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
            var authToken = this.Request.Headers.GetAuthorizationHeader();
            var version = this.Request.Headers.GetVersionRequestHeader();

            var items = new Dictionary<Guid, CartItemModel>();
            if (!cartItems.TryGetValue(authToken, out items))
            {
                throw CartException.UserHasNoItemsInCart();
            }

            var returnMessage = new HttpResponseMessage();
            switch (version)
            {
                case 1:
                    var userItems = items.Select(fx => fx.Value);
                    break;
                
                default:
                    throw CartException.InvalidVersionHeader();
            }

            return Task.FromResult(returnMessage);
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

            var bodyContent = await this.Request.Content.ReadAsStringAsync();
            switch (version)
            {
                case 1:
                    cartRequestBody = JsonConvert.DeserializeObject<CartItemV1>(bodyContent);
                    break;
                case 2:
                    cartRequestBody = JsonConvert.DeserializeObject<CartItemV2>(bodyContent);
                    break;
                default:
                    throw CartException.InvalidVersionHeader();
            }

            if (cartRequestBody == null)
            {
                throw CartException.InvalidCartItem();
            }

            var userItems = default(Dictionary<Guid, CartItemModel>);
            if (!cartItems.TryGetValue(authToken, out userItems))
            {
                userItems = new Dictionary<Guid,CartItemModel>();
                cartItems.Add(authToken, userItems);
            }

            if (userItems.ContainsKey(cartRequestBody.Id))
            {
                throw CartException.ItemAlreadyExistsInCart();
            }
            else
            {
                var item = default(CartItemBase);
                switch (version)
                {
                    case 1:
                        item = cartRequestBody as CartItemV1;
                        returnMessage = new HttpResponseMessage
                        {
                            Content = new JsonContent<CartItemV1>(item as CartItemV1)
                        };
                        break;
                    case 2:
                        item = cartRequestBody as CartItemV2;
                        returnMessage = new HttpResponseMessage
                        {
                            Content = new JsonContent<CartItemV2>(item as CartItemV2)
                        };
                        break;
                }
                userItems.Add(item.Id, item.ToModel());
            }

            return returnMessage;
        }
    }
}
