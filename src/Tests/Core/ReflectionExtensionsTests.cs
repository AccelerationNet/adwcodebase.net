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

        class DummyAttribute : Attribute { }

        [Test]
        public void GetAttribute() {
            var assembly = Assembly.GetAssembly(typeof(ReflectionExtensions));
            Assert.IsNotNull(assembly.GetAttribute<AssemblyCopyrightAttribute>());
            Assert.IsNull(assembly.GetAttribute<DummyAttribute>());
        }

    }
}
