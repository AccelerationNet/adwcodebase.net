# Acceleration.MappedReader

Helpers and convenience methods for pulling data out of `IDataReader`s.

Defines some extension methods on `IDataReader` to efficiently pull
out values based on column name, not column index.

* `T IDataReader.GetAs<T>(int)` - Fetch data from the reader as the
  given type. If mapping fails, throws a `BadMappingException` with
  details.
* `IMappedReader IDataReader.MappedReader()` - Make a helper object to fetch and
  cast values by name instead of ordinal
* `T IMappedReader.GetAs<T>(int)` - Cast and fetch the value from the
  reader, converting `DBNull` to `null`
* `T IMappedReader.Parse<T>(overrides=null)` - Make an instance of `T`
  based on matching column names to public, settable properties, with
  an optional mapping of property names to SQL columns for inexact
  matches

Example:

    using Acceleration.MappedReader;
	
	using(var reader = cmd.ExecuteReader()){
		var rdr = reader.MappedReader();
		while(reader.Read()){
			// support most primitive types and nullables
			var s = rdr.GetAs<string>("name");
			var d = rdr.GetAs<DateTime?>("date_entered");
			var b = rdr.GetAs<bool>("is_valid");

	        // make a new `MyType` filling in properties that have
            // matching column names
		    var o = rdr.Parse<MyType>();
			// make a new `MyType`, filling in `MyType.MyPropertyName`
			// from "colunmName"
			o = rdr.Parse<MyType>(new Dictionary<string,string>(){
				{"Id", "tbl_id"}
			});

		}
    }
