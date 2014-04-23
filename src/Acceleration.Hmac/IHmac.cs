using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Hmac
{
    /// <summary>
    /// Participates in HMAC hashing
    /// </summary>
    /// <remarks>
    /// To use:
    /// 
    ///  * make a viewmodel implementing `IHmac`
    ///  * make the validator inherits from `HMacValidator`
    ///  * annotate the properties you don't want to change with `[Hmac]`
    ///  * make the template pass the `Hash` and all fields marked with `[Hmac]`
    ///    back to the server as hidden fields
    ///
    /// Hashes get calculated automatically by `HmacFilter` on the way out.
    /// </remarks>
    public interface IHmac {
        string Hash { get; set; }
    }

    /// <summary>
    /// default implementation of `IHmac`
    /// </summary>
    public abstract class Hmac : IHmac {
        public string Hash { get; set; }
    }

}
