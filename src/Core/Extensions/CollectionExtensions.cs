using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Extensions {
    /// <summary>
    /// Extension methods for `System.Collections` classes
    /// </summary>
    public static class CollectionExtensions {
        /// <summary>
        /// Add to a collection in bulk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <returns>how many items were added</returns>
        public static int Add<T>(this ICollection<T> collection, IEnumerable<T> items) {
            if (collection == null) throw new ArgumentNullException("collection");

            var count = 0;
            foreach (var item in items) { collection.Add(item); count++; }
            return count;
        }

        /// <summary>
        /// Add to a collection in bulk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <returns>how many items were added</returns>
        public static int Add<T>(this ICollection<T> collection, params T[] items) {
            if (collection == null) throw new ArgumentNullException("collection");
            return collection.Add(items.AsEnumerable());
        }
    }
}
