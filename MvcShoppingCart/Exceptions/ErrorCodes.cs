//-----------------------------------------------------------------------
// <copyright file="JsonContent.cs" company="My Company">
//     Copyright (c) My Company. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcShoppingCart.Exceptions
{
    public class ErrorCodes
    {
        public ErrorCodes()
        { }

        /// <summary>
        /// Used to signify an unexpected exception in the service
        /// </summary>
        public static uint UnexpectedError { get { return 0; } }


        #region BadRequest (401) Errors
        /// <summary>
        /// Used when an Invalid Cart Item is encountered
        /// </summary>
        public static uint InvalidCartItem { get { return 1000; } }

        /// <summary>
        /// Used when trying to add an item to the cart that already exists
        /// </summary>
        public static uint ItemAlreadyInCart { get { return 1001; } }

        /// <summary>
        /// Used when trying to add an item to the cart that already exists
        /// </summary>
        public static uint InvalidVersionHeader { get { return 1002; } }
        #endregion

        #region UnAuthorized (403) Errors
        /// <summary>
        /// Used when trying to add an item to the cart that already exists
        /// </summary>
        public static uint InvalidAuthorizationHeader { get { return 2000; } }
        #endregion
    }
}