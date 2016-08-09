using System;
using System.Collections.Generic;
using System.Linq;
using FIMMonitoring.Common;
using FIMMonitoring.Domain.Repositories.IRepositories;
using FIMMonitoring.Domain.ViewModels.WCF;

namespace FIMMonitoring.Domain.Repositories
{
    public class ImportErrorRepository : AdvBaseRepository<ImportError>, IImportErrorRepository
    {
        public List<Guid> ProcessErrors(ErrorPack model)
        {
            Logger.Log.Info($"Start processing errors from system {model.SystemId}");

            List<Guid> Success = new List<Guid>();

            if (model.Errors.Any())
            {
                Logger.Log.Info($"Got {model.Errors.Count} new errors");
                foreach (var item in model.Errors)
                {
                    try
                    {
                        var importError = new ImportError()
                        {
                            ErrorLevel = item.ErrorLevel,
                            Cleared = false,
                            CreatedAt = DateTime.Now,
                            Description = item.Description,
                            ErrorDate = item.ErrorDate,
                            ErrorSource = item.ErrorSource,
                            ErrorType = item.ErrorType,
                            ErrorSendDate = item.ErrorsSendDate,
                            Filename = item.Filename,
                            ImportId = item.ImportId,
                            IsDownloaded = item.IsDownloaded,
                            IsParsed = item.IsParsed,
                            IsValidated = item.IsValidated,
                            SystemId = model.SystemId,
                            IsBusinessValidated = item.IsBusinessValidated
                        };

                        if (item.Customer.Id.HasValue)
                        {
                            var customer =
                                FimContext.FimCustomers.FirstOrDefault(e => e.ForeignId == item.Customer.Id && e.SystemId == model.SystemId) ??
                                new FimCustomer()
                                {
                                    ForeignId = item.Customer.Id.Value,
                                    Name = item.Customer.Name,
                                    SystemId = model.SystemId,
                                    FimCustomerCarriers = new List<FimCustomerCarrier>()
                                };

                            importError.FimCustomer = customer;
                        }

                        if (item.Carrier.Id.HasValue)
                        {
                            var carrier = FimContext.FimCarriers.FirstOrDefault(e => e.ForeignId == item.Carrier.Id && e.SystemId == model.SystemId) ??
                                          new FimCarrier()
                                          {
                                              ForeignId = item.Carrier.Id.Value,
                                              Name = item.Carrier.Name,
                                              SystemId = model.SystemId,
                                              CustomerCarriers = new List<FimCustomerCarrier>()
                                          };

                            if (item.Customer.Id.HasValue)
                            {
                                if (importError.FimCustomer.FimCustomerCarriers.FirstOrDefault(e => e.FimCarrier.ForeignId == item.Carrier.Id.Value && e.FimCarrier.SystemId == model.SystemId) == null)
                                {
                                    if (importError.FimCustomer.Id == 0)
                                    {
                                        importError.FimCustomer.FimCustomerCarriers.Add(new FimCustomerCarrier()
                                        {
                                            FimCarrier = carrier,
                                        });
                                    }
                                    else
                                    {
                                        FimContext.FimCustomerCarriers.Add(new FimCustomerCarrier()
                                        {
                                            FimCarrier = carrier,
                                            FimCustomer = importError.FimCustomer
                                        });
                                        FimContext.SaveChanges();
                                    }
                                }
                            }

                            importError.FimCarrier = carrier;
                        }

                        if (item.ImportSource.Id.HasValue)
                        {
                            var importSource =
                                FimContext.FimImportSources.FirstOrDefault(e => e.ForeignId == item.ImportSource.Id && e.SystemId == model.SystemId) ??
                                new FimImportSource()
                                {
                                    ForeignId = item.ImportSource.Id.Value,
                                    Name = item.ImportSource.Name,
                                    SystemId = model.SystemId,
                                    Carrier = importError.FimCarrier
                                };

                            importError.FimImportSource = importSource;
                        }

                        FimContext.ImportErrors.Add(importError);
                        FimContext.SaveChanges();

                        Success.Add(item.Guid);

                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error($"Error while adding ImportError from System {model.SystemId}, {e.Message}");
                        if (e.InnerException != null)
                        {
                            Logger.Log.Error($"Inner exception {e.InnerException.Message}");

                            if (e.InnerException.InnerException != null)
                                Logger.Log.Error($"Inner inner exception {e.InnerException.InnerException.Message}");
                        }
                    }
                }
                Logger.Log.Info($"Processed {model.Errors.Count} new errors");
            }
            else
            {
                Logger.Log.Info("No errors found in error pack. Exiting....");
            }

            return Success;
        }
    }
}
