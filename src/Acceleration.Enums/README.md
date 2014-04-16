# Acceleration.Enums

Helpers and convenience methods for working with enums. These are
exposed as extension methods, add `using Acceleration.Enums;` to get
access.

* `GetFlags<T>(T)` - Given a flag combination, list the individual enum
  values represented. Supports nullable enums
* `TryParseToEnum<T>(string, out T?)` - Try to parse a string to the
  enum, attempting a few string operations to match regardless of
  casing or whitespace
* `ParseToEnum<T>(string)` - Parse this string into an enum using
  `TryParseToEnum`, but throws exceptions on failure
