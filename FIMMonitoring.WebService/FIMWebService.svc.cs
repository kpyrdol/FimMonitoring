using System;
using System.Collections.Generic;
using FIMMonitoring.Domain.ViewModels.WCF;
using FIMMonitoring.Domain.Repositories;
using FIMMonitoring.Domain.Repositories.IRepositories;


namespace FIMMonitoring.WebService
{
    public class FIMWebService : IFIMWebService
    {
        protected IImportErrorRepository ImportErrorRepository;

        public FIMWebService()
        {
            ImportErrorRepository = new ImportErrorRepository();
        }

        public List<Guid> SendErrorPack(ErrorPack errors)
        {
            return ImportErrorRepository.ProcessErrors(errors);
        }
    }
}
