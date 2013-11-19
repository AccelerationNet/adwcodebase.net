using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Base : Acceleration.Testing.MSTestBase
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            AssemblyInitialize(context);
        }
    }
}
