using System;
using System.Collections.Specialized;
using System.ServiceProcess;
using FIMMonitoring.Common;
using Quartz;
using Quartz.Impl;

namespace FIMMonitoring.MainService
{
    public partial class MainService : ServiceBase
    {
        private static IScheduler _importScheduler;

        private readonly string JOBS_CONFIGURATION_PATH = @"config/jobs.xml";

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log.Info("Sending import errors service has been started at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var properties = GetConfigurationCollection();
            ISchedulerFactory factory = new StdSchedulerFactory(properties);
            _importScheduler = factory.GetScheduler();
            _importScheduler.Start();
        }

        protected override void OnStop()
        {
            Logger.Log.InfoFormat("Sending import errors service was shutdown at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void StandaloneStart()
        {
            OnStart(null);
            while (true) ;
        }

        private NameValueCollection GetConfigurationCollection()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "FimImportService";

            // job initialization plugin handles our xml reading, without it defaults are used
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";

            var configPath = AppDomain.CurrentDomain.BaseDirectory + JOBS_CONFIGURATION_PATH;
            properties["quartz.plugin.xml.fileNames"] = configPath;

            return properties;
        }
    }
}
