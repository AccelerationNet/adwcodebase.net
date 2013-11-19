using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Extensions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Core {
    [TestClass]
    public class ReflectionExtensionsTests : Base {

        class DummyAttribute : Attribute { }

        [TestMethod]
        public void GetAttribute() {
            var assembly = Assembly.GetAssembly(typeof(ReflectionExtensions));
            Assert.IsNotNull(assembly.GetAttribute<AssemblyCopyrightAttribute>());
            Assert.IsNull(assembly.GetAttribute<DummyAttribute>());
        }

    }
}
