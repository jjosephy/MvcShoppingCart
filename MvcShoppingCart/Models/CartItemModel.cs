using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Models
{
    public class CartItemModel
    {
        public CartItemModel()
        {
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }
    }
}