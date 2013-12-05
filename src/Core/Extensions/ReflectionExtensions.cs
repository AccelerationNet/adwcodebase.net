using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Acceleration.Extensions {
    /// <summary>
    /// Extension methods for `System.Reflection` classes
    /// </summary>
    public static class ReflectionExtensions {
        /// <summary>
        /// Fetch an attribute 
        /// </summary>
        /// <remarks>
        /// This is implemented in .NET 4 or 4.5, but the ifas-hr project
        /// is stuck at 3.5 to support it's server environment, so re-create.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider provider) where T : Attribute {
            if (provider == null) throw new ArgumentNullException("provider");
            var matches = provider.GetCustomAttributes(typeof(T), true);
            return matches.Select(m => m as T).FirstOrDefault(m => m != null);
        }
    }
}
