using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Acceleration.Enums
{
    public static class EnumExtensions
    {

        static void CheckEnumType(Type t) {            
            if (!t.IsEnum) { 
                throw new ArgumentException(
                    string.Format("Type must be an enum. {0} is not an enum.",
                        t)); 
            }
        }

        /// <summary>
        /// Given a flag combination, list the individual enum values represented
        /// </summary>
        /// <remarks>
        /// Adapted from http://stackoverflow.com/questions/4171140/iterate-over-values-in-flags-enum
        /// </remarks>
        /// <exception cref="ArgumentException">If the type is not an enum</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetFlags<T>(this T flags) where T : struct {
            CheckEnumType(typeof(T));
            var flagBits = Convert.ToUInt64(flags);
            foreach (T value in Enum.GetValues(typeof(T))) {
                var valueBits = Convert.ToUInt64(value);
                if ((flagBits & valueBits) == valueBits) {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Given a flag combination, list the individual enum values represented
        /// </summary>        
        /// <exception cref="ArgumentException">If the type is not an enum</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetFlags<T>(this Nullable<T> flags) where T : struct {
            return (flags == null) ? null : flags.Value.GetFlags();            
        }


        /// <summary>
        /// Parse this string into an enum.
        /// </summary>        
        /// <exception cref="ArgumentException">If the type is not an enum</exception>
        /// <exception cref="KeyNotFoundException">If the string is not a member of the enum</exception>
        /// <exception cref="ArgumentNullException">If the string is null or empty</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(this string name) where T : struct {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }
            Nullable<T> res;
            var ok = name.TryParseToEnum<T>(out res);

            if (ok) return res.Value;

            // no dice
            throw new KeyNotFoundException(string.Format("{0} is not defined in type of enum {1}", name, typeof(T).Name));
        }

        /// <summary>
        /// Try to parse a string to the enum
        /// </summary>
        /// <exception cref="ArgumentException">If the type is not an enum</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns>true if we parsed ok, false if no match was possible</returns>
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "used as an out parameter")]
        public static bool TryParseToEnum<T>(this string name, out Nullable<T> result)
        where T: struct{
            result = null;
            if (string.IsNullOrEmpty(name)) return false;

            var type = typeof(T);
            CheckEnumType(type);
            

            // got a direct match?
            if (Enum.IsDefined(type, name)) {
                result = (T)Enum.Parse(type, name);
            }
            else {
                // search for case-insensitve or space-insensitive matches
                var match = Enum.GetNames(type)
                    .FirstOrDefault(n => {
                        return n.Equals(name, StringComparison.OrdinalIgnoreCase)
                            || n.Equals(name.Replace(" ", String.Empty), StringComparison.OrdinalIgnoreCase);
                    });

                if (match != null) {
                    result = (T)Enum.Parse(type, match);
                    return true;
                }
            }

            return result != null;
        }
        /// <summary>
        /// Test if a string parses to and matches an enum value
        /// </summary>
        /// <exception cref="ArgumentException">If the type is not an enum</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">string to test</param>
        /// <param name="b">value to match</param>
        /// <returns>true if <paramref name="a"/> parses to <typeparamref name="T"/> and that value equals <paramref name="b"/>. False in all other cases, including nulls</returns>
        public static bool EnumEquals<T>(this string a, T b) where T:struct{
            Nullable<T> res = null;

            if (a.TryParseToEnum<T>(out res)) {
                return EqualityComparer<T>.Default.Equals(res.Value,b);
            }

            return false;
        }
    }
}
