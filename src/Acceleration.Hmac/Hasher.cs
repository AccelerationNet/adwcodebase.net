using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Acceleration.Collections;

namespace Acceleration.Hmac {

    /// <summary>
    /// Compute HMAC hashed
    /// </summary>
    public static class Hasher {
        /// <summary>
        /// how we convert to bytes for HMAC
        /// </summary>
        /// <remarks>
        /// we never do anything but base64 and compare the results, 
        /// so the specific encoder doesn't matter too much
        /// </remarks>
        static readonly Encoding Encoding = new System.Text.ASCIIEncoding();

        /// <summary>
        /// Compute a Hmac hash for the given salt and data points.
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="datum">the date we're hashing</param>
        /// <returns></returns>
        public static string ComputeHash(string secret, params object[] datum) {
            if (datum == null) throw new ArgumentNullException("datum");
            if (string.IsNullOrEmpty(secret))
                return ComputeHash("should use a real salt", datum);

            var key = Encoding.GetBytes(secret);
            using (var hmac = new HMACSHA256(key)) {

                var message = datum.Where(k => k != null)
                    .Select(k =>
                        k is IHmac ? ((IHmac)k).ComputeHash() : k.ToString()
                    )
                    .Where(k => !string.IsNullOrEmpty(k))
                    .SelectMany(Encoding.GetBytes);

                var hash = hmac.ComputeHash(message.ToArray());

                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// cache reflection results for what fields need to be HMAC'd
        /// </summary>
        static readonly IDictionary<Type, ICollection<MethodInfo>> HmacProperties = new Dictionary<Type, ICollection<MethodInfo>>();

        /// <summary>
        /// Compute a Hmac hash for all the properties tagged with
        /// `HmacAttribute`
        /// </summary>
        /// <remarks>Uses reflection and a naive cache to figure out what
        /// properties to include in the hash.</remarks>
        /// <param name="obj"></param>
        /// <param name="secret">uses type name by default</param>
        /// <returns></returns>
        public static string ComputeHash(this IHmac obj, string secret = null) {
            var type = obj.GetType();
            var keys = HmacProperties
                .Ensure(type, FindHmacProperties)
                .Select(m => m.Invoke(obj, null));
            var hash = ComputeHash(secret ?? type.FullName, keys.ToArray());
            if (obj.Hash == null) obj.Hash = hash;
            return hash;
        }        

        /// <summary>
        /// Scan the type for `HmacAttribute` properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static ICollection<MethodInfo> FindHmacProperties(Type type) {
            return type.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(HmacAttribute)))
                .Select(p => p.GetGetMethod())
                .ToList();
        }

        /// <summary>
        /// Recompute the hash and check it
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="secret">uses type name by default</param>
        /// <returns></returns>
        public static bool ValidateHash(this IHmac obj, string secret = null) {
            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (string.IsNullOrEmpty(obj.Hash)) {
                throw new ArgumentOutOfRangeException("obj.Hash",
                    "the object mush have a hash already");
            }

            return obj.Hash == ComputeHash(obj, secret);
        }
    }
}
