using MvcShoppingCart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Contracts
{
    public class CartItemV2 : CartItemBase
    {
        [JsonProperty(PropertyName = "color")]
        public string Color
        {
            get;
            set;
        }

        public override CartItemModel ToModel()
        {
            return new CartItemModel
            {
                Description = this.Description,
                Id = this.Id,
                Quantity = this.Quantity,
                Color = this.Color
            };
        }
    }
}