using log4net;
using log4net.Config;

namespace FIMMonitoring.Common
{
    public class Logger
    {
        private static ILog logger;

        public static ILog Log
        {
            get
            {
                if (logger == null)
                {
                    XmlConfigurator.Configure();
                    logger = LogManager.GetLogger("log4net");
                }

                return logger;
            }
        }
    }
}
