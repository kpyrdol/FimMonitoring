using FIMMonitoring.Domain.ViewModels.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using FIMMonitoring.Common;
using FIMMonitoring.Domain;
using FIMMonitoring.Service.Properties;

namespace FIMMonitoring.Service.Helpers
{
    public class WebServiceHelper
    {
        public List<Guid> Send(IQueryable<ServiceImportError> errors)
        {
            try
            {
                var retValues = new List<Guid>();

                if (errors.Any())
                {
                    var errorPack = new ErrorPack()
                    {
                        SystemId = Settings.Default.SystemId,
                        Errors = new List<Error>()
                    };

                    foreach (var item in errors)
                    {
                        var error = new Error()
                        {
                            Description = item.Description,
                            ErrorDate = item.ErrorDate,
                            ErrorLevel = item.ErrorLevel,
                            ErrorSource = item.ErrorSource,
                            ErrorType = item.ErrorType,
                            Filename = item.FileCheck?.FileName,
                            Guid = item.Guid,
                            ImportId = item.ImportId,
                            IsDownloaded = item.IsDownloaded,
                            IsParsed = item.IsParsed,
                            IsValidated = item.IsValidated,
                            Customer = new KeyValueClass(),
                            Carrier = new KeyValueClass(),
                            ImportSource = new KeyValueClass(),
                            IsBusinessValidated = item.IsBusinessValidated
                        };

                        

                        if (item.CustomerId.HasValue)
                        {
                            error.Customer = new KeyValueClass()
                            {
                                Name = item.CustomerName,
                                Id = item.CustomerId
                            };
                        }

                        if (item.CarrierId.HasValue)
                        {
                            error.Carrier = new KeyValueClass()
                            {
                                Id = item.CarrierId,
                                Name = item.CarrierName
                            };
                        }

                        if (item.SourceId.HasValue)
                        {
                            error.ImportSource = new KeyValueClass()
                            {
                                Name = item.SourceName,
                                Id = item.SourceId
                            };
                        }

                        errorPack.Errors.Add(error);
                    }


                    var client = new FimWS.FIMWebServiceClient();
                    retValues = client.SendErrorPack(errorPack);
                }

                return retValues;

            }
            catch (Exception e)
            {
                Logger.Log.Error($"Error while sending errors to WS {e.Message}");
                return new List<Guid>();
            }
        }
    }
}
