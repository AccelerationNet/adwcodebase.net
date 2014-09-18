using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Acceleration.Collections {
    /// <summary>
    /// Extension methods for `System.Collections.IDictionary`
    /// </summary>
    public static class DictionaryExtensions {

        /// <summary>
        /// Get a key from the dictionary, or return a default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue=default(TValue)) {
            if (dict == null) throw new ArgumentNullException("dict");
            return key != null && dict.Keys.Contains(key)
                ? dict[key] : defaultValue;
        }

        /// <summary>
        /// Get a key from the dictionary, or calculate a default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueFn"></param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFn) {
            if (dict == null) throw new ArgumentNullException("dict");
            if (valueFn == null)
                throw new ArgumentNullException("valueFn");
            return key != null && dict.Keys.Contains(key)
                ? dict[key] : valueFn(key);
        }

        /// <summary>
        /// Ensure this key is in this dictionary, setting it if needed
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueFn"></param>
        /// <returns></returns>
        public static TValue Ensure<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFn) {            
            if (dict == null) throw new ArgumentNullException("dict");
            if (key == null) throw new ArgumentNullException("key");
            if (valueFn == null)
                throw new ArgumentNullException("valueFn");

            if (!dict.Keys.Contains(key))
                dict[key] = valueFn(key);
            return dict[key];
        }

        /// <summary>
        /// Ensure this key is in this dictionary, setting it if needed
        /// </summary>
        /// <remarks>
        /// The specific overload resolves an ambigious type error when using
        /// `System.Collections.Generic.Dictionary`, which implements generic
        /// and regular `IDictionary`
        /// </remarks>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueFn"></param>
        /// <returns></returns>
        [SuppressMessage("Gendarme.Rules.Maintainability", 
            "AvoidUnnecessarySpecializationRule",
            Justification = "needed to disambiguated between `IDictionary` and `IDictionary<Tk,Tv>`, which are both implemented by `Dictionary<Tk,Tv>`")]
        public static TValue Ensure<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFn) {
            return Ensure((IDictionary<TKey, TValue>)dict, key, valueFn);
        }

        /// <summary>
        /// Ensure this key is in this dictionary, setting it if needed
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueFn"></param>
        /// <returns></returns>
        public static TValue Ensure<TKey, TValue>(this IDictionary dict, TKey key, Func<TKey, TValue> valueFn) {
            if (dict == null) throw new ArgumentNullException("dict");
            if (key == null) throw new ArgumentNullException("key");
            if (valueFn == null)
                throw new ArgumentNullException("valueFn");

            if (!dict.Contains(key))
                dict[key] = valueFn(key);
            return (TValue) dict[key];
        }

        /// <summary>
        /// Update the dictionary from another source. Overwrites keys that exist in both.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="other"></param>
        /// <returns>the updated dictionary</returns>
        public static IDictionary<TKey, TValue> Update<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> source) {
            if (dict == null) throw new ArgumentNullException("dict");
            if (source == null) throw new ArgumentNullException("source");

            foreach (var item in source) {
                dict[item.Key] = item.Value;
            }
            return dict;
        }
    }
}
