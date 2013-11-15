using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Acceleration.Extensions {
    public static class DataExtensions {

        /// <summary>
        /// Generate a mapping of column name to column ordinal
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IDictionary<string, int> ColumnOrdinalMap(this IDataReader reader) {
            if (reader == null) throw new ArgumentNullException("reader");

            var map = new Dictionary<string, int>();
            for (var i = 0; i < reader.FieldCount; i++) {
                map.Add(reader.GetName(i), i);
            }
            return map;
        }

        /// <summary>
        /// A mapping of types to IDataReader converter functions
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
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static T GetAs<T>(this IDataReader reader, int ordinal) {
            if (reader == null) throw new ArgumentNullException("reader");
            
            var isDbNull = reader.IsDBNull(ordinal);
            if (isDbNull) return default(T);

            var requestedType = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(requestedType);
            
            var typeToCoerce = underlyingType ?? requestedType;

            if (!Converters.ContainsKey(typeToCoerce)) {
                var supportedKeys = Converters.Keys
                    .Select(t => t.ToString())
                    .ToArray();
                throw new ArgumentException(
                    string.Format("type {0} is not supported, must be one of:\n\n{1}",
                        typeToCoerce, string.Join("\n", supportedKeys)));
            }

            return (T)Converters[typeToCoerce](reader, ordinal);
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
    }
}
