
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MvcShoppingCart.Contracts
{
    public class ErrorDetailsV1
    {
        [JsonProperty(PropertyName = "errorCode")]
        public uint ErrorCode
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "details")]
        public string Details
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "statusCode")]
        public HttpStatusCode StatusCode
        {
            get;
            set;
        }
    }
}