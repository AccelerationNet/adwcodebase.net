using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Acceleration.Extensions {
    public static class ReflectionExtensions {
        /// <summary>
        /// Fetch an attribute 
        /// </summary>
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
