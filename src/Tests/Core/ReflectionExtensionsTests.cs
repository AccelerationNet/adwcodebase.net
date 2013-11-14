using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Acceleration.Extensions;
using System.Reflection;

namespace Tests.Core {
    public class ReflectionExtensionsTests : Base {

        Assembly assembly;

        [SetUp]
        public void Setup() {
            assembly = Assembly.GetAssembly(typeof(ReflectionExtensions));
        }

        class DummyAttribute : Attribute { }

        [Test]
        public void GetAttribute() {
            Assert.IsNotNull(assembly.GetAttribute<AssemblyInformationalVersionAttribute>());

            Assert.IsNull(assembly.GetAttribute<DummyAttribute>());

        }

    }
}
