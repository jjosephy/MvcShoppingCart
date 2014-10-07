using MvcShoppingCart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Contracts
{
    public abstract class CartItemBase
    {
        public CartItemBase()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity
        {
            get;
            set;
        }

        public virtual CartItemModel ToModel()
        {
            throw new NotImplementedException("ToModel not implemented on base");
        }
    }
}