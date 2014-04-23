using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Hmac {
    [Serializable]
    public class HashMismatchException : Exception {
        public HashMismatchException() { }
        public HashMismatchException(string message) : base(message) { }
        public HashMismatchException(string message, Exception inner) : base(message, inner) { }
        protected HashMismatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
