using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FIMMonitoring.Common;
using FIMMonitoring.Common.Helpers;
using FIMMonitoring.Domain;
using FIMMonitoring.Domain.Enum;
using FIMMonitoring.Service.Properties;
using Quartz;

namespace FIMMonitoring.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class ImportCheckerJob : IJob
    {
        SoftLogsContext context = new SoftLogsContext(600);

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

                var fimContext = new FIMContext(600);
                Logger.Log.Info("Starting ImportCheckerJob");
                Logger.Log.Info("Getting list of imports that was not checked");

                var items = context.Imports.Where(e => !e.IsChecked && e.IsFinished && !e.MappingType.IsInternal && e.ImportDataType == ImportDataType.InvoicesData).ToList();
                if (items.Any())
                {
                    Logger.Log.Info($"{items.Count()} items not checked");
                    foreach (var item in items)
                    {

                        item.IsChecked = true;
                        Logger.Log.Info($"Processing import {item.Id}");
                        //if (!item.IsDownloaded || !item.IsParsed || !item.IsValidated)
                        //{
                        var importFile = new ServiceImportError()
                        {
                            CarrierId = item.ImportSource.CarrierMapping.CarrierId,
                            CustomerId = item.ImportSource.CustomerId,
                            ImportId = item.Id,
                            SourceId = item.ImportSourceID,
                            CarrierName = item.ImportSource.CarrierMapping.Carrier.Name,
                            CustomerName = item.ImportSource.Customer.Name,
                            SourceName = item.ImportSource.Name,
                            IsDownloaded = item.IsDownloaded,
                            IsParsed = item.IsParsed,
                            IsValidated = item.IsValidated,
                            ErrorLevel = ErrorLevel.None,
                            CreatedAt = DateTime.Now,
                            ErrorSource = ErrorSource.ImportTable,
                            Guid = Guid.NewGuid(),
                            IsBusinessValidated = true
                        };

                        importFile.ErrorDate = item.CreatedAt;

                        

                        if (!item.IsDownloaded)
                        {
                            Logger.Log.Info("File was not properly downloaded");
                            importFile.ErrorLevel = ErrorLevel.Critical;
                            importFile.ErrorType = ErrorType.DownloadFileError;
                        }

                        if (!item.IsValidated)
                        {
                            Logger.Log.Info("File was not validated");
                            importFile.ErrorLevel = ErrorLevel.Warning;
                            importFile.ErrorType = ErrorType.DataMismatchError;
                        }

                        if (!item.IsParsed)
                        {
                            Logger.Log.Info("File was not properly parsed");
                            importFile.ErrorLevel = ErrorLevel.Critical;
                            importFile.ErrorType = ErrorType.IncorrectFileFormatError;
                        }

                        BusinessValidation(ref importFile, item);
                        fimContext.ServiceImportErrors.Add(importFile);
                        fimContext.SaveChanges();
                        //}

                        context.SaveChanges();
                    }
                    Logger.Log.Info($"{items.Count()} items checked");
                }
                else
                {
                    Logger.Log.Info("No imports to check found");
                }

            }
            catch (DbUpdateConcurrencyException dex)
            {
                Logger.Log.Error(dex.Message);
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Import error service",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured in Import error service in system {Settings.Default.SystemId} {dex.Message}"
                };
                sender.Send(mail);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Import error service has encountered an error.", ex);
                var mail = new MailViewModel
                {
                    To = Settings.Default.MailAdmin,
                    Subject = "FIM Import error service",
                    CC = Settings.Default.MailToCC,
                    Body = $"An error occured in Import error service in system {Settings.Default.SystemId} {ex.Message}"
                };
                sender.Send(mail);
            }

        }


        private void BusinessValidation(ref ServiceImportError importError, Import import)
        {
            try
            {
                var invoices = context.Invoices.Where(e => e.ImportID == import.Id);

                List<string> Errors = new List<string>();

                foreach (var item in invoices)
                {
                    if (string.IsNullOrEmpty(item.InvoiceNumber))
                        Errors.Add($"No invoice number in invoice {item.Id}");

                    if (item.ValidationMode == ValidationModes.ValidateOnly)
                    {
                        if (item.NetAmount == 0)
                            Errors.Add($"0 net amount in invoice {item.Id}");
                    }
                    else
                    {
                        if (item.ValidatedNetAmount == 0)
                            Errors.Add($"0 validated net amount in invoice {item.Id}");
                    }

                    if (string.IsNullOrEmpty(item.ReceiverNumber))
                        Errors.Add($"No receiver number in invoice {item.Id}");

                    foreach (var waybill in item.Waybills)
                    {
                        if (string.IsNullOrEmpty(waybill.WaybillNumber))
                            Errors.Add($"No waybill number in waybill {waybill.Id}");

                        if (waybill.NetAmount == 0)
                            Errors.Add($"0 net amount in waybill {waybill.Id}");

                        if (string.IsNullOrEmpty(waybill.TransportService))
                            Errors.Add($"No transport service in waybill {waybill.Id}");

                        foreach (var charge in waybill.Charges)
                        {
                            if (string.IsNullOrEmpty(charge.Description))
                                Errors.Add($"No description in charge {charge.Id}");

                            if (charge.TotalAmount == 0)
                                Errors.Add($"0 total amount in charge {charge.Id}");

                            if (charge.MappedCode != null && charge.MappedCode.Equals("OTHER"))
                                Errors.Add($"Mapped code OTHER in charge {charge.Id}");
                        }
                    }
                }

                if (Errors.Any())
                {
                    importError.ErrorLevel = ErrorLevel.Warning;
                    importError.Description = string.Join("<br/>", Errors);
                    importError.ErrorLevel = ErrorLevel.Warning;
                    importError.IsBusinessValidated = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
               
        }
    }
}