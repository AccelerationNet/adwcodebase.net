using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceleration.Testing
{
    /// <summary>
    /// Base class for MSTest tests
    /// </summary>
    [TestClass]
    public class MSTestBase : Base
    {
        protected static void AssemblyInitialize(TestContext context) {
            SetupLog4net();
        }
    }
}
