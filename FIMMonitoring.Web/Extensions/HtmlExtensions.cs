using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIMMonitoring.Common;
using FIMMonitoring.Domain;
using FIMMonitoring.Domain.Enum;

namespace FIMMonitoring.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static string AsDate(this DateTime item, bool withHours = false)
        {
            if (withHours)
                return item.ToString("yyyy-MM-dd HH:mm:ss");

            return item.ToString("yyyy-MM-dd");
        }

        public static string AsDate(this DateTime? item, bool withHours = false)
        {
            if (!item.HasValue)
                return string.Empty;


            if (withHours)
                return item.Value.ToString("yyyy-MM-dd HH:mm:ss");

            return item.Value.ToString("yyyy-MM-dd");
        }

        public static MvcHtmlString GetAppUrl(this HtmlHelper html)
        {

            var Request = HttpContext.Current.Request;
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return new MvcHtmlString($"{Request.Url.Scheme}://{Request.Url.Authority}{url.Content("~")}");
        }

        public static MvcHtmlString GetServerUrl(this HtmlHelper html)
        {

            var Request = HttpContext.Current.Request;
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return new MvcHtmlString($"{Request.Url.Scheme}://{Request.Url.Authority}");
        }

        public static MvcHtmlString ToLabel(this ErrorLevel level)
        {
            string cssClass = level == ErrorLevel.Critical ? "danger" : level == ErrorLevel.Warning ? "warning" : "success";
            return new MvcHtmlString($"<span class=\"label label-{cssClass}\">{level.GetEnumName()}</span>");
        }

        public static MvcHtmlString TrueFalseLabel(this HtmlHelper helper, bool val, string text = null)
        {
            var cssClass = val ? "label-success" : "label-danger";
            var value = text ?? (val ? "Yes" : "No");
            return new MvcHtmlString($"<span class=\"label {cssClass}\">{value}</span>");
        }

        public static string GetIsLinkActive(string action, string controller, ViewContext context)
        {
            var act = context.RouteData.Values["action"];
            var ctr = context.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(action))
                return controller.Equals(ctr) ? "active" : string.Empty;

            return controller.Equals(ctr) && action.Equals(act) ? "active" : string.Empty;
        }

        public static string GetCssClass(this FilesListItemViewModel model)
        {
            var css = model.IsCleared ? " text-grey " : "";

            if (string.IsNullOrEmpty(css))
            {
                css = $"{css} {(model.ErrorLevel == ErrorLevel.Warning ? " text-warning " : "")}";
                css = $"{css} {(model.ErrorLevel == ErrorLevel.Critical ? " text-critical" : "")}";
                css = $"{css} {(model.ErrorLevel == ErrorLevel.None ? " text-green" : "")}";
            }


            if (model.SourceDisabled)
                css = $"{css} source-disabled";

            return css;
        }

        public static string GetCssClass(this CustomerImportViewModel model)
        {
            var css = "success";

            if (model.Carriers.Any(e => e.CountErrorsNotDisabled > 0))
                return "danger";

            if (model.Carriers.Any(e => e.CountWarningsNotDisabled > 0))
                return "warning";

            return css;
        }

        public static string GetCssClass(this CarrierDetailsViewModel model)
        {
            var css = "success";

            if (model.CountErrorsNotDisabled > 0)
                return "danger";

            if (model.CountWarningsNotDisabled > 0)
                return "warning";

            return css;
        }

    }
}