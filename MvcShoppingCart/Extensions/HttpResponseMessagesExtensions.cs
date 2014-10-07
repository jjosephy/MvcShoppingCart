

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
        const string VersionHeader = "x-api-version";
        const string Authorization = "authorization";
        const string AuthenticationHeader = "apitoken";

        public static string GetAuthorizationHeader(this HttpRequestHeaders headers)
        {
            var authHeader = headers.Where(header =>
                   header.Key.Equals(Authorization, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var exception = CartException.InvalidAuthorizationHeader();
            ValidateHeader(authHeader, exception);

            var headerValue = authHeader.Value.FirstOrDefault().Split(' ');
            if (headerValue.Count() != 2 || 
                !headerValue[0].Equals(AuthenticationHeader, StringComparison.OrdinalIgnoreCase))
            {
                throw exception;
            }

            return headerValue[1];
        }

        public static uint GetVersionRequestHeader(this HttpRequestHeaders headers)
        {
            var version = headers.Where(header =>
                   header.Key.Equals(VersionHeader, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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