
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MvcShoppingCart.Models;

namespace MvcShoppingCart.Contracts
{
    public class CartItemV1 : CartItemBase
    {
        public override CartItemModel ToModel()
        {
            return new CartItemModel
            {
                Description = this.Description,
                Id = this.Id,
                Quantity = this.Quantity
            };
        }
    }
}