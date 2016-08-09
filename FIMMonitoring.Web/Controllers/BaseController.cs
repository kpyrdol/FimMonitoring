using System.IO;
using System.Web.Mvc;
using FIMMonitoring.Domain;
using FIMMonitoring.Web.Extensions;
using FIMMonitoring.Web.Properties;

namespace FIMMonitoring.Web.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class BaseController : Controller
    {
        protected FIMContext Context = new FIMContext(Settings.Default.CommandTimeout);
        protected SoftLogsContext SoftLogsContext = new SoftLogsContext(Settings.Default.CommandTimeout);

        public int PageSize => 25;
        public BaseController(){}

        private void MergeMessage(string alertType, string message)
        {
            if (!TempData.Keys.Contains(alertType))
                TempData.Add(alertType, message);
            else
                TempData[alertType] = TempData[alertType] + "<br />" + message;
        }

        public void Attention(string message)
        {
            MergeMessage(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            MergeMessage(Alerts.SUCCESS, message);
        }

        public void Warning(string message)
        {
            MergeMessage(Alerts.WARNING, message);
        }

        public void Information(string message)
        {
            MergeMessage(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            MergeMessage(Alerts.ERROR, message);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}