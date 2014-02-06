using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Versioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Versioning {
    
    [TestClass]
    public class VersioningTests : Base {
        public const string INFORMATIONAL_VERSION = "asdf";

        [TestMethod]
        public void Get() {
            var info = VersionInfo.Get(Assembly.GetExecutingAssembly());            
            Assert.IsNotNull(info);
            Assert.AreEqual(INFORMATIONAL_VERSION, info.InformationalVersion);
            Assert.IsNotNull(info.Version);
        }
    }
}
