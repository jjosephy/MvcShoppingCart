using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MvcShoppingCart.Tests.Extensions
{
    public static class TestUtilty
    {
        const int passwordLength = 8;
        const int alphaNumericalCharsAllowed = 3;

        public static string GenerateRandomString()
        {
            return Membership.GeneratePassword(passwordLength, alphaNumericalCharsAllowed);
        }

        public static string Base64Encode(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(bytes);
        }
    }
}
