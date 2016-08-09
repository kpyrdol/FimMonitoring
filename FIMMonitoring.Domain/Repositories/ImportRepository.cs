using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FIMMonitoring.Domain.Enum;
using FIMMonitoring.Domain.ViewModels;
using PagedList;

namespace FIMMonitoring.Domain
{
    public class ImportRepository : BaseRepository, IImportRepository
    {
        public int SystemId { get; set; }

        public ImportRepository(int systemId)
        {
            SystemId = systemId;
        }

        public ImportError GetById(int id)
        {
            return FimContext.ImportErrors.FirstOrDefault(e => e.Id == id);
        }

        public Import GetImportById(int id)
        {
            return SoftLogsContext.Set<Import>().FirstOrDefault(e => e.Id == id);
        }

        public Import GetFileToDownload(int id)
        {
            var item = FimContext.ImportErrors.FirstOrDefault(e => e.Id == id);
            return SoftLogsContext.Set<Import>().Include(i => i.ImportSource.CarrierMapping.Carrier)
                    .FirstOrDefault(i => i.Id == item.ImportId);
        }

        public ListImportsViewModel GetCustromersStatuses()
        {

            return new ListImportsViewModel()
            {
                Systems = FimContext.FimSystems.Select(e=>new SystemViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Customers = FimContext.FimCustomers.OrderBy(c=>c.Name).Where(c=>c.SystemId == e.Id).Select(c=>new CustomerImportViewModel()
                    {
                        CustomerId = c.Id,
                        CustomerName = c.Name,
                        SystemCss = c.System.CssStyle,
                        Carriers = c.FimCustomerCarriers.Select(ce=>new CarrierViewModel()
                        {
                            Name = ce.FimCarrier.Name,
                            CarrierId = ce.FimCarrierId,
                            Disabled = ce.FimCarrier.FimImportSources.All(imp => imp.Disabled),
                            CountWarnings = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && !fi.Cleared),
                            CountWarningsNotDisabled = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && !fi.Cleared && !fi.FimImportSource.Disabled),
                            CountErrors = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && !fi.Cleared),
                            CountErrorsNotDisabled = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && !fi.Cleared && !fi.FimImportSource.Disabled),
                            CountOk = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.None && fi.FimCustomerId == c.Id && !fi.Cleared),
                            CountWarningsCleared = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && fi.Cleared),
                            CountErrorsCleared = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && fi.Cleared),
                        }).OrderBy(ce=>ce.Name).ToList()
                    }).OrderBy(ce => ce.CustomerName).ToList()
                }).OrderBy(ce => ce.Name).ToList(),
                Customers = FimContext.FimCustomers.OrderBy(c => c.Name).Select(c => new CustomerImportViewModel()
                {
                    CustomerId = c.Id,
                    CustomerName = c.Name,
                    SystemCss = c.System.CssStyle,
                    Carriers = c.FimCustomerCarriers.Select(ce => new CarrierViewModel()
                    {
                        Name = ce.FimCarrier.Name,
                        CarrierId = ce.FimCarrierId,
                        Disabled = ce.FimCarrier.FimImportSources.Any(imp => imp.Disabled),
                        CountWarnings = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && !fi.Cleared),
                        CountWarningsNotDisabled = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && !fi.Cleared && !fi.FimImportSource.Disabled),
                        CountErrors = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && !fi.Cleared),
                        CountErrorsNotDisabled = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && !fi.Cleared && !fi.FimImportSource.Disabled),
                        CountOk = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.None && fi.FimCustomerId == c.Id && !fi.Cleared),
                        CountWarningsCleared = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == c.Id && fi.Cleared),
                        CountErrorsCleared = ce.FimCarrier.ImportErrors.Count(fi => fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == c.Id && fi.Cleared),
                    }).OrderBy(ce => ce.Name).ToList()
                }).OrderByDescending(ce => ce.Carriers.Sum(cee=>cee.CountErrorsNotDisabled)).ThenByDescending(ce=>ce.Carriers.Sum(cee=>cee.CountWarnings)).ToList()
            };

            //var customers = new ListImportsViewModel()
            //{
            //    Customers = FimContext.FimCustomers.OrderBy(e=>e.Name).Where(e=>e.SystemId == SystemId).Select(e=>new CustomerImportViewModel()
            //    {
            //        CustomerId = e.Id,
            //        CustomerName = e.Name,
            //        Carriers = e.FimCustomerCarriers.Select(c=>new CarrierViewModel()
            //        {
            //            Name = c.FimCarrier.Name,
            //            CarrierId = c.FimCarrierId,
            //            CountErrors = c.FimCarrier.ImportErrors.Count(fi=>fi.ErrorLevel == ErrorLevel.Critical && fi.FimCustomerId == e.Id && !fi.Cleared),
            //            CountOk = c.FimCarrier.ImportErrors.Count(fi=>fi.ErrorLevel == ErrorLevel.None && fi.FimCustomerId == e.Id && !fi.Cleared),
            //            CountWarnings = c.FimCarrier.ImportErrors.Count(fi=>fi.ErrorLevel == ErrorLevel.Warning && fi.FimCustomerId == e.Id && !fi.Cleared)
            //        }).OrderBy(c=>c.Name).ToList()
            //    }).ToList()

                //Customers = SoftLogsContext.Customers.OrderBy(e => e.Name).Where(i => i.ImportSources.Any(imp => !imp.CarrierMapping.MappingType.IsInternal)).Select(e => new CustomerImportViewModel()
                //{
                //    CustomerId = e.Id,
                //    CustomerName = e.Name,
                //    Carriers = e.CustomerCarriers.Where(cc =>
                //        SoftLogsContext.ImportSources.Any(ims => ims.CustomerId == e.Id
                //            && ims.CarrierMapping.CarrierId == cc.CarrierId
                //            && !ims.CarrierMapping.MappingType.IsInternal)
                //              ).Select(c => new CarrierViewModel()
                //              {
                //            //CountWarnings = SoftLogsContext.Imports.Count(i=> i.IsFinished && (!i.IsValidated) && i.ImportSource.CarrierMapping.CarrierId == c.CarrierId && i.ImportSource.IsEnabled && !i.ImportSource.CarrierMapping.MappingType.IsInternal && i.ImportSource.CustomerId == e.Id),
                //            //CountErrors = SoftLogsContext.Imports.Count(i=> i.IsFinished && (!i.IsDownloaded || !i.IsParsed) && i.ImportSource.CarrierMapping.CarrierId == c.CarrierId && i.ImportSource.IsEnabled && !i.ImportSource.CarrierMapping.MappingType.IsInternal && i.ImportSource.CustomerId == e.Id),
                //            //CountWarnings = FimContext.ImportErrors.Count(ie => ie.CarrierId == c.CarrierId && ie.CustomerId == e.Id && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Warning),
                //            //CountErrors = FimContext.ImportErrors.Count(ie => ie.CarrierId == c.CarrierId && ie.CustomerId == e.Id && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Critical),
                //            Name = c.Carrier.Name,
                //                  CarrierId = c.CarrierId
                //              }).OrderBy(c => c.Name).ToList()
                //}).ToList()
            //};

            //foreach (var customer in customers.Customers)
            //{
            //    foreach (var carrier in customer.Carriers)
            //    {
            //        carrier.CountWarnings =
            //            FimContext.ImportErrors.Count(
            //                ie => ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Warning);
            //        carrier.CountErrors =
            //            FimContext.ImportErrors.Count(
            //                ie => ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Critical);
            //        carrier.CountOk = FimContext.ImportErrors.Count(
            //                ie => ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared && ie.ErrorLevel == ErrorLevel.None);
            //    }
            //}

            //return customers;
        }

        public async Task<List<ImportSourceViewModel>> GetImportSources(FilesListViewModel model)
        {
            var customerId = model.CustomerId;
            var carrierId = model.CarrierId;

            return await FimContext.FimImportSources.Where(e => e.CarrierId == carrierId && e.Carrier.CustomerCarriers.Any(a => a.FimCustomerId == customerId))
                    .Select(e => new ImportSourceViewModel()
                    {
                        Name = e.Name,
                        SourceId = e.Id,
                        CustomerId = customerId,
                        CarrierId = e.CarrierId,
                        Checked = model.SelectedSources.Contains(e.Id),
                        Enabled = !e.Disabled,
                        CountWarnings = e.ImportErrors.Count(im=>im.ErrorLevel == ErrorLevel.Warning && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && !im.Cleared),
                        CountErrors = e.ImportErrors.Count(im => im.ErrorLevel == ErrorLevel.Critical && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && !im.Cleared),
                        CountOk = e.ImportErrors.Count(im => im.ErrorLevel == ErrorLevel.None && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && !im.Cleared),
                        CountWarningsCleared = e.ImportErrors.Count(im => im.ErrorLevel == ErrorLevel.Warning && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && im.Cleared),
                        CountErrorsCleared = e.ImportErrors.Count(im => im.ErrorLevel == ErrorLevel.Critical && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && im.Cleared),
                        CountOkCleared = e.ImportErrors.Count(im => im.ErrorLevel == ErrorLevel.None && im.FimCustomerId == customerId && im.FimCarrierId == carrierId && im.Cleared),
                    }).OrderBy(e=>e.Name).ToListAsync();

            //var items = await SoftLogsContext.ImportSources.Where(
            //        e => e.CustomerId == customerId && e.CarrierMapping.CarrierId == carrierId && !e.CarrierMapping.MappingType.IsInternal).Select(e => new ImportSourceViewModel()
            //        {
            //            Name = e.Name,
            //            SourceId = e.Id,
            //            CustomerId = e.CustomerId,
            //            CarrierId = e.CarrierMapping.CarrierId,
            //        }).ToListAsync();

            //foreach (var item in items)
            //{
            //    item.Checked = model.SelectedSources.Contains(item.SourceId);
            //    item.CountErrors =
            //        FimContext.ImportErrors.Count(
            //            ie =>
            //                ie.ErrorLevel == ErrorLevel.Critical && ie.CustomerId == item.CustomerId &&
            //                ie.CarrierId == item.CarrierId && ie.SourceId == item.SourceId);
            //    item.CountWarnings =
            //        FimContext.ImportErrors.Count(
            //            ie =>
            //                ie.ErrorLevel == ErrorLevel.Warning && ie.CustomerId == item.CustomerId &&
            //                ie.CarrierId == item.CarrierId && ie.SourceId == item.SourceId);
            //    item.CountOk =
            //        FimContext.ImportErrors.Count(
            //            ie =>
            //                ie.ErrorLevel == ErrorLevel.None && ie.CustomerId == item.CustomerId &&
            //                ie.CarrierId == item.CarrierId && ie.SourceId == item.SourceId);
            //}

            //return items;
        }

        public async Task<bool> SetStatus(int id, ErrorStatus status)
        {
            var item = await FimContext.ImportErrors.FirstOrDefaultAsync(e => e.Id == id);
            if (item == null)
                return false;

            item.Cleared = status == ErrorStatus.CLEAR;
            await FimContext.SaveChangesAsync();
            return true;
        }

        public async Task<ErrorDetailsViewModel> GetDetails(int id)
        {
            return
                await
                    FimContext.ImportErrors.Where(e => e.Id == id)
                        .Select(e => new ErrorDetailsViewModel()
                        {
                            ErrorLevel = e.ErrorLevel,
                            ErorrDate = e.ErrorDate,
                            Id = e.Id,
                            ErrorType = e.ErrorType,
                            IsCleared = e.Cleared,
                            IsValidated = e.IsValidated,
                            SourceName = e.FimImportSource.Name,
                            IsDownloaded = e.IsDownloaded,
                            IsParsed = e.IsParsed,
                            ProcessedDate = e.CreatedAt,
                            SendErorrDate = e.ErrorSendDate,
                            ImportId = e.ImportId,
                            SystemUrl = e.System.Url
                        })
                        .FirstOrDefaultAsync();
        }

        public CustomerViewModel GetCustomerDetails(int id)
        {

            return FimContext.FimCustomers.Where(e => e.Id == id).Select(e => new CustomerViewModel()
            {
                CustomerId = e.Id,
                CustomerName = e.Name,
                SystemName = e.System.Name,
                Carriers = e.FimCustomerCarriers.Select(cc=>new CarrierDetailsViewModel()
                {
                    CarrierId = cc.FimCarrierId,
                    Name = cc.FimCarrier.Name,
                    CountWarnings = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Warning && ie.FimCustomerId == e.Id && !ie.Cleared),
                    CountWarningsNotDisabled = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Warning && ie.FimCustomerId == e.Id && !ie.Cleared && !ie.FimImportSource.Disabled),
                    CountErrors = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Critical && ie.FimCustomerId == e.Id && !ie.Cleared),
                    CountErrorsNotDisabled = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Critical && ie.FimCustomerId == e.Id && !ie.Cleared && !ie.FimImportSource.Disabled),
                    CountOk = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.None && ie.FimCustomerId == e.Id && !ie.Cleared),
                    CountWarningsCleared = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Warning && ie.FimCustomerId == e.Id && ie.Cleared),
                    CountErrorsCleared = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.Critical && ie.FimCustomerId == e.Id && ie.Cleared),
                    CountOkCleared = cc.FimCarrier.ImportErrors.Count(ie => ie.ErrorLevel == ErrorLevel.None && ie.FimCustomerId == e.Id && ie.Cleared),
                    Sources = cc.FimCarrier.FimImportSources.Select(im => new ImportSourceViewModel()
                    {
                        Name = im.Name,
                        SourceId = im.Id,
                        CarrierId = im.CarrierId,
                        CustomerId = e.Id,
                        Disabled = im.Disabled,
                        CountWarnings = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Warning && ime.FimCustomerId == e.Id && !ime.Cleared),
                        CountWarningsNotDisabled = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Warning && ime.FimCustomerId == e.Id && !ime.Cleared && !ime.FimImportSource.Disabled),
                        CountErrors = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Critical && ime.FimCustomerId == e.Id && !ime.Cleared),
                        CountErrorsNotDisabled = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Critical && ime.FimCustomerId == e.Id && !ime.Cleared && !ime.FimImportSource.Disabled),
                        CountOk = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.None && ime.FimCustomerId == e.Id && !ime.Cleared),
                        CountWarningsCleared = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Warning && ime.FimCustomerId == e.Id && ime.Cleared),
                        CountErrorsCleared = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.Critical && ime.FimCustomerId == e.Id && ime.Cleared),
                        CountOkCleared = im.ImportErrors.Count(ime => ime.ErrorLevel == ErrorLevel.None && ime.FimCustomerId == e.Id && ime.Cleared),
                    }).OrderBy(s=>s.Name).ToList()
                }).OrderByDescending(s=>s.CountErrorsNotDisabled).ThenByDescending(s=>s.CountWarningsNotDisabled).ToList()
            }).FirstOrDefault();

            //var customer = SoftLogsContext.Customers.Where(e => e.Id == id).Select(e => new CustomerViewModel()
            //{
            //    CustomerId = e.Id,
            //    CustomerName = e.Name,
            //    Carriers = e.CustomerCarriers.OrderBy(c => c.Carrier.Name).Where(cc =>
            //              SoftLogsContext.ImportSources.Any(ims => ims.CustomerId == e.Id
            //                  && ims.CarrierMapping.CarrierId == cc.CarrierId
            //                  && !ims.CarrierMapping.MappingType.IsInternal)
            //            ).Select(c => new CarrierDetailsViewModel()
            //            {
            //                Name = c.Carrier.Name,
            //                CarrierId = c.CarrierId,
            //                Sources = SoftLogsContext.ImportSources.Where(ims => ims.CarrierMapping.CarrierId == c.CarrierId && ims.CustomerId == e.Id && !ims.CarrierMapping.MappingType.IsInternal).Select(cs => new ImportSourceViewModel()
            //                {
            //                    Name = cs.Name,
            //                    //CountWarnings = SoftLogsContext.Imports.Count(i => i.IsFinished && (!i.IsValidated) && i.ImportSource.CarrierMappingId == cs.CarrierMappingId && i.ImportSource.IsEnabled && !i.ImportSource.CarrierMapping.MappingType.IsInternal && i.ImportSource.CustomerId == e.Id),
            //                    //CountErrors = SoftLogsContext.Imports.Count(i => i.IsFinished && (!i.IsDownloaded || !i.IsParsed) && i.ImportSource.CarrierMappingId == cs.CarrierMappingId  && i.ImportSource.IsEnabled && !i.ImportSource.CarrierMapping.MappingType.IsInternal),
            //                    //CarrierId = c.CarrierId,
            //                    //CountWarnings = FimContext.ImportErrors.Count(ie => ie.CarrierId == c.CarrierId && ie.CustomerId == e.Id && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Warning),
            //                    //CountErrors = FimContext.ImportErrors.Count(ie => ie.CarrierId == c.CarrierId && ie.CustomerId == e.Id && !ie.Cleared && ie.ErrorLevel == ErrorLevel.Critical),
            //                    SourceId = cs.Id,
            //                    CustomerId = e.Id,
            //                    CarrierId = cs.CarrierMapping.CarrierId
            //                }).ToList()
            //            }).Where(es => es.Sources.Any()).ToList()

            //}).FirstOrDefault();

            //foreach (var carrier in customer.Carriers)
            //{
            //    foreach (var source in carrier.Sources)
            //    {
            //        //source.CountWarnings =
            //        //FimContext.ImportErrors.Count(
            //        //    ie =>
            //        //        ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared &&
            //        //        ie.ErrorLevel == ErrorLevel.Warning && ie.SourceId == source.SourceId);
            //        //source.CountErrors =
            //        //    FimContext.ImportErrors.Count(
            //        //        ie =>
            //        //            ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared &&
            //        //            ie.ErrorLevel == ErrorLevel.Critical && ie.SourceId == source.SourceId);
            //        //source.CountOk =
            //        //    FimContext.ImportErrors.Count(
            //        //        ie =>
            //        //            ie.CarrierId == carrier.CarrierId && ie.CustomerId == customer.CustomerId && !ie.Cleared &&
            //        //            ie.ErrorLevel == ErrorLevel.None && ie.SourceId == source.SourceId);
            //    }
            //}

            //return customer;
        }

        public async Task<FilesListViewModel> GetFiles(FilesListViewModel model)
        {
            //var carrier = await SoftLogsContext.Carriers.FirstOrDefaultAsync(e => e.Id == model.CarrierId);
            //var customer = await SoftLogsContext.Customers.FirstOrDefaultAsync(e => e.Id == model.CustomerId);
            var carrier = await FimContext.FimCarriers.FirstOrDefaultAsync(e => e.Id == model.CarrierId);
            var customer = await FimContext.FimCustomers.FirstOrDefaultAsync(e => e.Id == model.CustomerId);

            model.CarrierName = carrier.Name;
            model.CustomerName = customer.Name;
            var system = FimContext.FimSystems.FirstOrDefault(e => e.Id == SystemId);
            model.SystemName = system.Name;

            if(model.SourceErrors == null)
                model.SourceErrors = new List<SourceError>();

            if (model.ErrorLevels != null && model.ErrorLevels.Any())
            {
                model.SelectedSources.ForEach(e =>
                {
                    model.SourceErrors.Add(new SourceError()
                    {
                        ErrorLevels = model.ErrorLevels,
                        SourceId = e
                    });
                });
            }

            var items = FimContext.ImportErrors.Where(e => e.FimCarrierId == model.CarrierId && e.FimCustomerId== model.CustomerId);
            var returnItems = FimContext.ImportErrors.Take(0);

            if (model.SelectedSources.Any())
            {

                foreach (var item in model.SelectedSources)
                {
                    var sourcesItems = items.Where(e => e.FimImportSourceId == item);
                    var levels = model.SourceErrors.FirstOrDefault(e => e.SourceId == item);

                    if (levels?.ErrorLevels != null && levels.ErrorLevels.Any())
                    {
                        sourcesItems = sourcesItems.Where(e => levels.ErrorLevels.Contains(e.ErrorLevel));
                    }
                    returnItems = returnItems.Concat(sourcesItems);
                }

            }

            model.Files = returnItems.Select(e => new FilesListItemViewModel()
            {
                Id = e.Id,
                ImportId = e.ImportId,
                ErrorType = e.ErrorType,
                ErrorLevel = e.ErrorLevel,
                ErrorDate = e.ErrorDate,
                ErrorSendDateTime = e.ErrorSendDate,
                ProcessedDate = e.CreatedAt,
                IsCleared = e.Cleared,
                SourceName = e.FimImportSource.Name,
                ErrorSource = e.ErrorSource,
                Description = e.Description,
                IsDownloaded = e.IsDownloaded,
                IsValidated = e.IsValidated,
                IsParsed = e.IsParsed,
                SystemId = system.Id,
                SystemName = system.Name,
                SystemUrl = system.Url,
                SourceDisabled = e.FimImportSource != null && e.FimImportSource.Disabled
        }).OrderByDescending(e => e.ErrorDate).ToPagedList(model.Page.Value, model.PageSize);

            return model;
        }

        public async Task<bool> ChangeImportSourceStatus(int id, bool status)
        {
            try
            {
                var item = FimContext.FimImportSources.FirstOrDefault(e => e.Id == id);
                if (item == null)
                    return false;
                item.Disabled = status;
                await FimContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
