using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Extensions
{
    public interface ICartRequestContext
    {
        string UserToken
        {
            get;
            set;
        }

        uint Version
        {
            get;
            set;
        }
    }
}