using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Hmac {
    /// <summary>
    /// Marks this attribute as being part of the HMAC hash
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false,
        Inherited = true)]
    public class HmacAttribute : Attribute { }
}
