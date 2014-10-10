
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
            var cartContext = this.CreateCartContext();
            var items = new Dictionary<Guid, CartItemModel>();
            if (!cartItems.TryGetValue(cartContext.UserToken, out items))
            {
                throw CartException.UserHasNoItemsInCart();
            }

            var returnMessage = new HttpResponseMessage();
            switch (cartContext.Version)
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
            var cartContext = this.CreateCartContext();
            CartItemBase cartRequestBody = null;
            var returnMessage = new HttpResponseMessage();

            var bodyContent = await this.Request.Content.ReadAsStringAsync();
            switch (cartContext.Version)
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
            if (!cartItems.TryGetValue(cartContext.UserToken, out userItems))
            {
                userItems = new Dictionary<Guid,CartItemModel>();
                cartItems.Add(cartContext.UserToken, userItems);
            }

            if (userItems.ContainsKey(cartRequestBody.Id))
            {
                throw CartException.ItemAlreadyExistsInCart();
            }
            else
            {
                var item = default(CartItemBase);
                switch (cartContext.Version)
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

        /// <summary>
        /// Handler for deleting all items in the cart
        /// </summary>
        /// <returns>An HttpResponseMessage that signals status</returns>
        public HttpResponseMessage Delete()
        {
            var cartContext = this.CreateCartContext();

            var items = new Dictionary<Guid,CartItemModel>();
            if (!cartItems.TryGetValue(cartContext.UserToken, out items))
            {
                throw CartException.UserHasNoItemsInCart();
            }

            if (!cartItems.ContainsKey(cartContext.UserToken))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("item not in user collection exception")
                    });
            }

            cartItems.Remove(cartContext.UserToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(string id)
        {
            var cartContext = this.CreateCartContext();

            var items = new Dictionary<Guid,CartItemModel>();
            if (!cartItems.TryGetValue(cartContext.UserToken, out items))
            {
                throw CartException.UserHasNoItemsInCart();
            }

            Guid itemId = Guid.Empty;
            if (!Guid.TryParse(id, out itemId))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("invalid item id exception")
                    });
            }

            if (!cartItems[cartContext.UserToken].ContainsKey(itemId))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("item not in user collection exception")
                    });
            }

            cartItems[cartContext.UserToken].Remove(itemId);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
