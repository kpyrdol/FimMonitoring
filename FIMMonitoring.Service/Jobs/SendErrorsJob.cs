using FIMMonitoring.Domain;
using Quartz;
using System;
using System.Linq;
using FIMMonitoring.Common;
using FIMMonitoring.Common.Helpers;
using FIMMonitoring.Service.Helpers;
using FIMMonitoring.Service.Properties;

namespace FIMMonitoring.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class SendErrorsJob : IJob
    {
        protected WebServiceHelper WSHelper;

        public SendErrorsJob()
        {
            WSHelper = new WebServiceHelper();
        }

        public void Execute(IJobExecutionContext jobContext)
        {
            var sender = new MailSender()
            {
                MailFrom = Settings.Default.MailFrom,
                MailFromDisplayName = Settings.Default.MailFromDisplayName,
                MailFromPassword = Settings.Default.MailFromPassword,
                MailHost = Settings.Default.MailHost,
                MailPort = Settings.Default.MailPort,
                MailSsl = Settings.Default.MailSsl
            };
            try
            {
                Logger.Log.Info("Starting SendErrorsJob");
                using (var context = new FIMContext(600))
                {
                    var items = context.ServiceImportErrors.Where(e => !e.IsSent).OrderBy(e=>e.Id).Take(500);

                    Logger.Log.Info($"Got {items.Count()} file to process.");

                    if (items.Any())
                    {
                        Logger.Log.Info("Sending files.");

                        var itemsOk = WSHelper.Send(items);

                        Logger.Log.Info($"All files sent. Got {itemsOk.Count} accepted files");

                        if (itemsOk.Any())
                        {
                            Logger.Log.Info("Accepting files");
                            foreach (var item in itemsOk)
                            {
                                var errorItem = context.ServiceImportErrors.FirstOrDefault(e => e.Guid.Equals(item));
                                if (errorItem != null)
                                    errorItem.IsSent = true;

                                context.SaveChanges();
                            }
                            Logger.Log.Info("All files accepted. Exiting...");
                        }
                    }
                    else
                    {
                        Logger.Log.Info("No new files to send. Exiting...");
                    }

                   
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Error while processing serviceImportErrors {e.Message}");
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Import error service",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured while sending items to WS {e.Message}"
                };
                sender.Send(mail);
            }
        }
    }
}