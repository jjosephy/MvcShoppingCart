using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Logging
{
    public class EventId
    {
        public static int MethodStartEventId = 5000;
        public static int MethodEndEventId = 5001;

        public static int InvalidCartEventId = 6000;
    }
}