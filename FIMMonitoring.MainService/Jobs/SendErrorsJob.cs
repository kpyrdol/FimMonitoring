using System;
using System.Linq;
using System.Linq.Expressions;
using FIMMonitoring.Common;
using FIMMonitoring.Common.Helpers;
using FIMMonitoring.Domain;
using FIMMonitoring.MainService.Properties;
using Quartz;

namespace FIMMonitoring.MainService.Jobs
{
    [DisallowConcurrentExecution]
    public class SendErrorsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Logger.Log.Info("Starting SendErrorsJob");

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
                var fimContext = new FIMContext(600);

                var items = fimContext.ImportErrors.Where(e => !e.IsChecked).ToList();
                Logger.Log.Info($"Got {items.Count()} items to check");
                foreach (var item in items)
                {
                    item.IsChecked = true;
                    if (Settings.Default.SendEmail && (item.FimImportSource == null || !item.FimImportSource.Disabled))
                    {
                        var mail = new MailViewModel
                        {
                            To = Settings.Default.MailTo,
                            Subject =
                                $"FIM Import error service {item.System.Url} Client: {item.FimCustomer?.Name}  Carrier {item.FimCarrier?.Name}",
                            CC = Settings.Default.MailToCC,
                            Body =
                                $"An error occurred while importing data.<br/>{getHtmlRow("Client", item.FimCustomer?.Name)} {getHtmlRow("Carrier", item.FimCarrier?.Name)} {getHtmlRow("Import date", item.ErrorDate?.ToString("yyyy-MM-dd HH:mm"))} {getHtmlRow("Source", item.FimImportSource?.Name)} {getHtmlRow("Error source", item.ErrorSource.GetEnumName())}"
                        };


                        if (!item.IsDownloaded || !item.IsValidated || !item.IsParsed || !item.IsBusinessValidated)
                        {
                            Logger.Log.Info($"Import {item.Id} is not correct");
                            if (!item.IsDownloaded)
                            {
                                mail.Body = $"{mail.Body} File was not properly downloaded<br/>";
                            }

                            if (!item.IsValidated)
                            {
                                mail.Body = $"{mail.Body} File was not validated<br/>";
                            }

                            if (!item.IsParsed)
                            {
                                mail.Body = $"{mail.Body} File was not properly parsed<br/>";
                            }

                            if (!item.IsBusinessValidated)
                            {
                                mail.Body =
                                    $"{mail.Body} <br/><strong>Details of business validation:</strong><br/>{item.Description}";
                            }

                            mail.Body = $"{mail.Body} <br/><br/><a href=\"{item.System.Url}\\Import\">Go to WEB</a>";

                            Logger.Log.Info("Sending email messsage");
                            sender.Send(mail);
                        }
                        item.ErrorSendDate = DateTime.Now;
                    }
                    else
                    {
                        Logger.Log.Info("Email sending turned off in config");
                    }
                    fimContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Erorr in SendErrorsJob {e.Message}");
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Send errors job error",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured in Send errors job {e.Message}"
                };
                sender.Send(mail);
            }
        }

        private string getHtmlRow(string title, string val)
        {
            return string.IsNullOrEmpty(val) ? string.Empty : $"{title}: <strong>{val}</strong><br/>";
        }
    }
}
