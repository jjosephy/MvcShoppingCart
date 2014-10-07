using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MvcShoppingCart.Controllers
{
    public abstract class ControllerBase : ApiController
    {
        protected virtual Task<HttpResponseMessage> Post()
        {
            throw new NotSupportedException("POST Method not supported");
        }

        protected virtual Task<HttpResponseMessage> Get()
        {
            throw new NotSupportedException("GET Method not supported");
        }

        protected virtual Task<HttpResponseMessage> Put()
        {
            throw new NotSupportedException("PUT Method not supported");
        }

        protected virtual Task<HttpResponseMessage> Delete()
        {
            throw new NotSupportedException("DELETE Method not supported");
        }
    }
}