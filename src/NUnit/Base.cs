using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using System.Reflection;

namespace Acceleration.NUnit
{
    public class Base
    {
        static bool oneTimeSetup;
        protected ILog Log { get; private set; }

        protected DateTime TEST_DATETIME { get; private set; }
        protected decimal TEST_DECIMAL { get; private set; }
        protected double TEST_DOUBLE { get; private set; }
        protected int TEST_INT { get; private set; }
        protected string TEST_STRING { get; private set; }

        public Base() {
            Log = LogManager.GetLogger(GetType());
            TEST_DATETIME = DateTime.Now;
            TEST_DECIMAL = 1.21M;
            TEST_DOUBLE = Math.PI;
            TEST_INT = 12345;
            TEST_STRING = "test";
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup() {
            if (!oneTimeSetup) {
                OneTimeFixtureSetup();
                oneTimeSetup = true;
            }
        }        

        protected virtual void OneTimeFixtureSetup() {
            using (var cfg = Assembly.GetExecutingAssembly().GetManifestResourceStream("Acceleration.NUnit.log4net.config")) {
                log4net.Config.XmlConfigurator.Configure(cfg);
            }
            Log.Info("Logging configured");
        }
    }
}
