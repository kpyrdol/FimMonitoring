
using System.ServiceProcess;

namespace FIMMonitoring.MainService
{
    static class IMainService
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var service = new MainService();
            service.StandaloneStart();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MainService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
