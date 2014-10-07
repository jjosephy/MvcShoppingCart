using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace MvcShoppingCart.Owin
{
    public class ShoppingCartMiddleWare : OwinMiddleware
    {
        public ShoppingCartMiddleWare(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
        }
    }
}