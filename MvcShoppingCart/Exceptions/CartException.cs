
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MvcShoppingCart.HttpContentFormatter;
using MvcShoppingCart.Contracts;
using Newtonsoft.Json;

namespace MvcShoppingCart.Exceptions
{
    public class CartException : HttpResponseException
    {
        public CartException(
            string message, 
            uint errorCode = 0x0,
            HttpStatusCode statusCode = HttpStatusCode.OK) :
                base(CreateMessage(message, errorCode, statusCode))
        {
        }

        public static HttpResponseException ItemAlreadyExistsInCartException()
        {
            const string message = "Item already exists in the cart. Use PUT to update cart.";
            return new CartException(message, ErrorCodes.InvalidCartItem, HttpStatusCode.BadRequest);
        }

        public static HttpResponseException InvalidCartItemException()
        {
            const string message = "Item passed to the cart is invalid";
            return new CartException(message, ErrorCodes.InvalidCartItem, HttpStatusCode.BadRequest);
        }

        public static HttpResponseException InvalidVersionHeader()
        {
            const string message = "Invalid Version Header provided";
            return new CartException(message, ErrorCodes.InvalidVersionHeader, HttpStatusCode.BadRequest);
        }

        public static HttpResponseException InvalidAuthorizationHeader()
        {
            const string message = "Invalid Authorization Header provided";
            return new CartException(message, ErrorCodes.InvalidAuthorizationHeader, HttpStatusCode.Forbidden);
        }

        public static HttpResponseException UnexpectedCartException(string exceptionMessage)
        {
            const string message = "Unexpected Cart Exception occurred";
            return new CartException(
                string.Format("{0}:{1}", message, exceptionMessage),
                ErrorCodes.UnexpectedError, 
                HttpStatusCode.InternalServerError);
        }

        static HttpResponseMessage CreateMessage(
            string message, 
            uint errorCode,
            HttpStatusCode statusCode)
        {

            return new HttpResponseMessage
            {
                Content = new JsonContent<ErrorDetailsV1>(
                    new ErrorDetailsV1
                    {
                        Details = message,
                        ErrorCode = errorCode,
                        StatusCode = statusCode
                    }),
                StatusCode = statusCode
            };
        }
    }
}