using System;
using System.Collections.Generic;
using FIMMonitoring.Domain.ViewModels.WCF;

namespace FIMMonitoring.Domain.Repositories.IRepositories
{
    public interface IImportErrorRepository :  IRepository<ImportError>
    {
        List<Guid> ProcessErrors(ErrorPack model);
    }
}
