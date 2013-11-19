using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Acceleration.Testing
{
    /// <summary>
    /// Base testing class, with log4net config and some 
    /// sigil values to use in testing.
    /// </summary>
    public class Base
    {
        protected ILog Log { get; private set; }
        protected DateTime TEST_DATETIME { get; private set; }
        protected decimal TEST_DECIMAL { get; private set; }
        protected double TEST_DOUBLE { get; private set; }
        protected int TEST_INT { get; private set; }
        protected string TEST_STRING { get; private set; }

        public Base()
        {
            Log = LogManager.GetLogger(GetType());
            TEST_DATETIME = DateTime.Now;
            TEST_DECIMAL = 1.21M;
            TEST_DOUBLE = Math.PI;
            TEST_INT = 12345;
            TEST_STRING = "test";
        }

        protected static void SetupLog4net()
        {
            using (var cfg = Assembly.GetExecutingAssembly().GetManifestResourceStream("Acceleration.Testing.log4net.config"))
            {
                log4net.Config.XmlConfigurator.Configure(cfg);
            }            
        }
    }
}
