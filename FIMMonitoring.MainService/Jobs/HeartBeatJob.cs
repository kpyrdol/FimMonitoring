using System;
using System.Linq;
using FIMMonitoring.Common;
using FIMMonitoring.Common.Helpers;
using FIMMonitoring.Domain;
using FIMMonitoring.MainService.Properties;
using Quartz;

namespace FIMMonitoring.MainService.Jobs
{
    [DisallowConcurrentExecution]
    public class HeartBeatJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Logger.Log.Info("Starting HeartBeatJob");
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
                var fimContext = new FIMContext();

                var systems = fimContext.FimSystems.ToList();
                Logger.Log.Info($"Got {systems.Count} systems");
                foreach (var system in systems)
                {
                    var item = fimContext.ImportErrors.Where(e => e.SystemId == system.Id)
                            .OrderByDescending(e => e.Id)
                            .FirstOrDefault();

                    if (item != null)
                    {

                        if (item.CreatedAt < DateTime.Now.AddHours(-4))
                        {
                            Logger.Log.Info("Old entries found. Sending email");
                            var mail = new MailViewModel
                            {
                                To = Settings.Default.MailAdmin,
                                Subject = "FIM Import errors",
                                CC = Settings.Default.MailToCC,
                                Body =
                                    $"No new imports sent by environment {system.Name}. Last received import at {item.CreatedAt.ToString("yyyy-MM-dd HH:mm")}"
                            };
                            sender.Send(mail);
                        }
                        else
                        {
                            Logger.Log.Info($"OK in environment {system.Name}");
                        }

                    }
                    else
                    {
                        Logger.Log.Info($"No imports found in environment {system.Name}");
                    }
                }

                Logger.Log.Info("Finished checking all systems");
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Error occured while processing HearBeat {e.Message}");
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Import error service",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured in Import error service - Hearbeat {e.Message}"
                };
                sender.Send(mail);
            }
        }
    }
}
