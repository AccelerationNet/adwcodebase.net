using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Build;
using NUnit.Framework;

namespace Tests.Build {
    public class VersioningTests : Base {
        [Test]
        public void VersionInfo() {
            var info = Versioning.VersionInfo(Assembly.GetAssembly(typeof(Versioning)));
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.InformationalVersion);
            Assert.IsNotNull(info.Version);
        }

        [Test]
        public void VersionInfoNoInfo() {
            var info = Versioning.VersionInfo(Assembly.GetExecutingAssembly());
            Assert.IsNotNull(info);
            Assert.IsNull(info.InformationalVersion);
            Assert.IsNotNull(info.Version);
        }
    }
}
