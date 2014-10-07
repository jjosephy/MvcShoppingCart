

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using MvcShoppingCart.Exceptions;
using System.Web.Http;
using System.Text;

namespace MvcShoppingCart.Extensions
{
    public static class HttpResponseMessagesExtensions
    {
        const string versionHeader = "x-api-version";
        const string authorization = "authorization";

        public static string GetAuthorizationHeader(this HttpRequestHeaders headers)
        {
            var authHeader = headers.Where(header =>
                   header.Key.Equals(authorization, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var exception = CartException.InvalidAuthorizationHeader();
            ValidateHeader(authHeader, exception);

            var headerValue = authHeader.Value.FirstOrDefault().Split(' ');
            if (headerValue.Count() != 2)
            {
                throw exception;
            }

            var bytes = Convert.FromBase64String(headerValue[1]);
            return Encoding.UTF8.GetString(bytes);
        }

        public static uint GetVersionRequestHeader(this HttpRequestHeaders headers)
        {
            var version = headers.Where(header =>
                   header.Key.Equals(versionHeader, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            ValidateHeader(version, CartException.InvalidVersionHeader());

            uint parsed = 0;
            if (UInt32.TryParse(version.Value.FirstOrDefault(), out parsed))
            {
                return parsed; 
            }

            throw new ArgumentException("could not parse version");
        }

        private static void ValidateHeader(
            KeyValuePair<string, IEnumerable<string>> header, 
            HttpResponseException exception)
        {
            if (header.Key == null || header.Value == null || !header.Value.Any())
            {
                throw exception;
            }
        }
    }
}