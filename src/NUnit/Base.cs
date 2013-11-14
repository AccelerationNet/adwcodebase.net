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

        public Base() {
            Log = LogManager.GetLogger(GetType());
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
