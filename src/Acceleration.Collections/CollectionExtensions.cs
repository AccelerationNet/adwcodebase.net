using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Collections {
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
            if (items == null) return 0;

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

        /// <summary>
        /// pick a random element from the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="rand">if null, a new `Random` will be created.</param>
        /// <returns></returns>
        public static T RandomElement<T>(this ICollection<T> coll, Random rand = null) {
            if (coll == null) { throw new ArgumentNullException("coll"); }
            if (rand == null) { rand = new Random(); }
            return coll.ElementAt(rand.Next(coll.Count));
        }

        /// <summary>
        /// pick N unique random elements from the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="count">maximum number of elements to return.</param>
        /// <param name="rand">if null, a new `Random` will be created.</param>
        /// <returns></returns>
        public static ICollection<T> RandomElement<T>(this ICollection<T> coll, int count, Random rand = null) {
            if (coll == null) { throw new ArgumentNullException("coll"); }
            if (count <= 0) { throw new ArgumentOutOfRangeException("count", "must be greater than 0"); }
            if (count >= coll.Count()) return new List<T>(coll);

            if (rand == null) { rand = new Random(); }            
            var chosen = new List<T>(count);
            while (chosen.Count < count) {
                var el = coll.RandomElement(rand);
                if (!chosen.Contains(el)) {
                    chosen.Add(el);
                }
            }

            return chosen;
        }
    }
}
