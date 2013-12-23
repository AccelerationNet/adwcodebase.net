using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Extensions {
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
                throw new ArgumentNullException("defaultValueFn");

            if (!dict.Keys.Contains(key))
                dict[key] = valueFn(key);
            return dict[key];
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
                throw new ArgumentNullException("defaultValueFn");

            if (!dict.Contains(key))
                dict[key] = valueFn(key);
            return (TValue) dict[key];
        }
    }
}
