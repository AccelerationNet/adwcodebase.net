using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceleration.Testing
{
    /// <summary>
    /// Base class for NUnit tests
    /// </summary>
    [TestFixture]
    public class NUnitBase : Base
    {
        static bool oneTimeSetup;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if (!oneTimeSetup)
            {
                OneTimeFixtureSetup();
                oneTimeSetup = true;
            }
        }

        protected virtual void OneTimeFixtureSetup()
        {
            SetupLog4net();
            Log.Debug("Logging started");
        }
    }
}
