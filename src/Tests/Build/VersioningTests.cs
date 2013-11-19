using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Build;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Build {
    
    [TestClass]
    public class VersioningTests : Base {
        public const string INFORMATIONAL_VERSION = "asdf";

        [TestMethod]
        public void VersionInfo() {
            var info = Versioning.VersionInfo(Assembly.GetExecutingAssembly());            
            Assert.IsNotNull(info);
            Assert.AreEqual(INFORMATIONAL_VERSION, info.InformationalVersion);
            Assert.IsNotNull(info.Version);
        }
    }
}
