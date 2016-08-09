using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FIMMonitoring.Domain;
using FIMMonitoring.Domain.Enum;
using FIMMonitoring.Domain.Repositories;
using FIMMonitoring.Web.Extensions;

namespace FIMMonitoring.Web.Controllers
{
    [Auth(AppRoles = ApplicationRoles.Administrator)]
    public class ImportController : BaseController
    {
        protected IImportRepository ImportRepository;

        public ImportController()
        {
            ImportRepository = new ImportRepository(1);
        }


        public ActionResult Index(int? page)
        {
            var model = ImportRepository.GetCustromersStatuses();
            return View(model);
        }


        public ActionResult Customer(int id)
        {
            var model = ImportRepository.GetCustomerDetails(id);
            return View(model);
        }


        public async Task<ActionResult> Files(int CarrierId, int CustomerId, List<ErrorLevel> ErrorLevels, List<int> SelectedSources)
        {
            var ImportSources = await ImportRepository.GetImportSources(new FilesListViewModel() { CarrierId = CarrierId, CustomerId = CustomerId});

            var model = new FilesListViewModel()
            {
                CarrierId = CarrierId,
                CustomerId = CustomerId,
                ErrorLevels = ErrorLevels??new List<ErrorLevel>(),
                SelectedSources = SelectedSources ?? ImportSources.Select(e => e.SourceId).ToList()
            };
            return await Files(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Files(FilesListViewModel model)
        {
            try
            {
                model.PageSize = PageSize;
                model.Page = model.Page ?? 1;

                model.ImportSources = await ImportRepository.GetImportSources(model);

                model = await ImportRepository.GetFiles(model);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = "OK", html = RenderPartialViewToString("_FilesLIst", model) }, JsonRequestBehavior.AllowGet);
                }
                return View(model);
            }
            catch (Exception e)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = "error", error = e.Message}, JsonRequestBehavior.AllowGet);
                }
                Error($"Error while processing your request {e.Message}");
                return RedirectToAction("Index");
            }
        }


        public ActionResult Download(int id)
        {
            var doc = ImportRepository.GetFileToDownload(id);
            if (doc == null)
                return new EmptyResult();
            return File(doc.RawContent.Content, "text/plain", doc.Filename);
        }

        [Auth(AppRoles =ApplicationRoles.Administrator)]
        public ActionResult DownloadPdf(int id)
        {
            var doc = ImportRepository.GetFileToDownload(id);

            if (doc == null || doc.DocumentContentBlobID == null)
                return new EmptyResult();

            return File(doc.DocumentContent.Content, doc.DocumentContentMimeType, doc.DocumentContentName);
        }

        public ActionResult DownloadValidation(int id)
        {
            var item = ImportRepository.GetById(id);
            var reportContent = new ReportRepository(SoftLogsContext).GenerateValidationErrorsReport(item.ImportId.Value);
            var import = ImportRepository.GetImportById(item.ImportId.Value);

            return File(reportContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                string.Format("{0}_import_validation_{1}.xlsx", import.ImportSource.Customer.Name.ToUpper().Replace(" ", "_"),
                    DateTime.Now.ToString("yyyyMMdd_HHmm")));
        }

        [HttpPost]
        public async Task<ActionResult> SetStatus(int id, ErrorStatus status)
        {
            await ImportRepository.SetStatus(id, status);
            return Json(new { status = "OK"}, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ShowDetails(int id)
        {
            var model = await ImportRepository.GetDetails(id);
            return PartialView("_Details", model);
        }

        public async Task<ActionResult> ShowDescription(int id)
        {
            var model = ImportRepository.GetById(id);
            return PartialView("_Description", model.Description);
        }

        [HttpPost]
        public async Task<JsonResult> SetSourceStatus(int id, bool enabled)
        {
            if(await ImportRepository.ChangeImportSourceStatus(id, !enabled))
                return Json(new { status = "OK", message = $"{(enabled ? "Monitoring turned on" : "Monitoring turned off")}" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = "error", error = "An error accured while changing status" }, JsonRequestBehavior.AllowGet);
        }
    }
}