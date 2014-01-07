using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Acceleration.Core.Data;

namespace Acceleration.Extensions {
    /// <summary>
    /// Extension methods for `System.Data` classes
    /// </summary>
    public static class DataExtensions {

        /// <summary>
        /// Generate a mapping of column name to column ordinal
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>map of column name to ordinal</returns>
        public static IDictionary<string, int> ColumnOrdinalMap(this IDataReader reader) {
            if (reader == null) throw new ArgumentNullException("reader");

            var map = new Dictionary<string, int>();
            for (var i = 0; i < reader.FieldCount; i++) {
                map.Add(reader.GetName(i), i);
            }
            return map;
        }

        /// <summary>
        /// A mapping of types to `IDataReader` converter functions
        /// </summary>
        static IDictionary<Type, Func<IDataReader, int, object>> Converters = MakeConverters();

        /// <summary>
        /// Builds the <see cref="Converters"/> mapping
        /// </summary>
        /// <returns></returns>
        static IDictionary<Type, Func<IDataReader, int, object>> MakeConverters() {
            var map = new Dictionary<Type, Func<IDataReader, int, object>>();

            map[typeof(DateTime)] = (r, i) => r.GetDateTime(i);
            map[typeof(decimal)] = (r, i) => r.GetDecimal(i);
            map[typeof(double)] = (r, i) => r.GetDouble(i);
            map[typeof(bool)] = (r, i) => r.GetBoolean(i);
            map[typeof(int)] = (r, i) => r.GetInt32(i);
            map[typeof(string)] = (r, i) => r.GetString(i);            

            return map;
        
        }

        /// <summary>
        /// Fetch data from the reader as the given type
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requestedType"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        [SuppressMessage("Gendarme.Rules.Portability",
            "MonoCompatibilityReviewRule",
            Justification = "checking type == null is fine")]
        public static object GetAs(this IDataReader reader, Type requestedType, int ordinal) {
            if (reader == null) throw new ArgumentNullException("reader");
            if (requestedType == null) throw new ArgumentNullException("requestedType");

            var isDbNull = reader.IsDBNull(ordinal);
            if (isDbNull) 
                return requestedType.IsValueType 
                    ? Activator.CreateInstance(requestedType) 
                    : null;

            var underlyingType = Nullable.GetUnderlyingType(requestedType);

            var typeToCoerce = underlyingType ?? requestedType;

            if (!Converters.ContainsKey(typeToCoerce)) {
                var supportedKeys = Converters.Keys
                    .Select(t => t.ToString())
                    .ToArray();
                // put together a useful error message
                throw new ArgumentException(
                    string.Format("type {0} is not supported, must be one of:\n\n{1}",
                        typeToCoerce, string.Join("\n", supportedKeys)));
            }

            return Converters[typeToCoerce](reader, ordinal);

        }
        /// <summary>
        /// Fetch data from the reader as the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static T GetAs<T>(this IDataReader reader, int ordinal) {
            if (reader == null) throw new ArgumentNullException("reader");
            return (T)reader.GetAs(typeof(T), ordinal);
        }

        public static T GetAs<T>(this IDataReader reader, string column, IDictionary<string, int> map) {
            if (reader == null) throw new ArgumentNullException("reader");
            if (map == null) throw new ArgumentNullException("map");
            if (string.IsNullOrEmpty(column))
                throw new ArgumentNullException("reader");
            if (!map.ContainsKey(column))
                throw new ArgumentOutOfRangeException("column", "column not contained in the map");

            return reader.GetAs<T>(map[column]);
        }

        /// <summary>
        /// Make a helper object to fetch and cast values by name 
        /// instead of ordinal
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IMappedReader MappedReader(this IDataReader reader) {
            if (reader == null) throw new ArgumentNullException("reader");
            return new MappedReader(reader);
        }
    }
}
