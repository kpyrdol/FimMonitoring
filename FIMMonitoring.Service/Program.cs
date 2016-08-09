using System.Data.Entity;
using System.ServiceProcess;
using FIMMonitoring.Domain;

namespace FIMMonitoring.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Database.SetInitializer<SoftLogsContext>(null);

#if DEBUG
            var service = new FIMMonitoringService();
            service.StandaloneStart();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FIMMonitoringService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
