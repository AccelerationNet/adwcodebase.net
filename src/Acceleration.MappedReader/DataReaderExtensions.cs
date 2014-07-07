using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Acceleration.MappedReader {
    /// <summary>
    /// Extension methods for `System.Data` classes
    /// </summary>
    public static class DataReaderExtensions {

        /// <summary>
        /// Generate a mapping of column name to column ordinal
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>map of column name to ordinal</returns>
        internal static IDictionary<string, int> ColumnOrdinalMap(this IDataReader reader) {
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
        internal static object GetAs(this IDataReader reader, Type requestedType, int ordinal) {
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
            try {
                return Converters[typeToCoerce](reader, ordinal);
            }
            catch (InvalidCastException ex){
                var actual = reader.GetValue(ordinal);
                var name = reader.GetName(ordinal);
                throw new BadMappingException(actual, requestedType, name, ex);
            }
            

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

    [Serializable]
    public class BadMappingException : Exception {
        public object Value { get; private set; }
        public Type RequestedType { get; private set; }
        public string ColumnName { get; private set; }

        public BadMappingException() { }
        public BadMappingException(string message) : base(message) { }
        public BadMappingException(string message, Exception inner) : base(message, inner) { }
        protected BadMappingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
        public BadMappingException(object value, Type type, string columnName, Exception inner)
            : base(BuildMessage(value, type, columnName), inner) {
                
            Value = value;
            RequestedType = type;
            ColumnName = columnName;
        }

        static string BuildMessage(object value, Type type, string columnName) {
            var valueType = value == null ? null : value.GetType();
            return string.Format(
                "Failed to convert column `{0}` from {3} `{1}` to {2}",
                    columnName, value, type, valueType);
        }
    }
}
