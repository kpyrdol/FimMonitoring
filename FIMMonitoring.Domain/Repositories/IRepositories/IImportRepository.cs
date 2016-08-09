using System.Collections.Generic;
using System.Threading.Tasks;
using FIMMonitoring.Domain.Enum;
using FIMMonitoring.Domain.ViewModels;

namespace FIMMonitoring.Domain
{
    public interface IImportRepository
    {
        ImportError GetById(int id);
        Import GetFileToDownload(int id);
        ListImportsViewModel GetCustromersStatuses();
        CustomerViewModel GetCustomerDetails(int id);
        Task<FilesListViewModel> GetFiles(FilesListViewModel model);
        Task<List<ImportSourceViewModel>> GetImportSources(FilesListViewModel model);
        Task<bool> SetStatus(int id, ErrorStatus status);
        Task<ErrorDetailsViewModel> GetDetails(int id);
        Import GetImportById(int id);
        Task<bool> ChangeImportSourceStatus(int id, bool status);
    }
}
