
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using MvcShoppingCart.Controllers;

namespace MvcShoppingCart.Tests.Owin
{
    class TestAssemblyResolver : DefaultAssembliesResolver
    {
        /// <summary>
        /// Gets the list of assemblies to load controllers for
        /// </summary>
        /// <returns>A collection of assemblies to load</returns>
        public override ICollection<Assembly> GetAssemblies()
        {
            // TODO: figure out a better way to load assemblies
            return new Assembly[1]
            { 
                typeof(CartController).Assembly
            };
        }
    }
}
