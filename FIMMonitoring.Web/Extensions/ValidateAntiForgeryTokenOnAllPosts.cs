using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FIMMonitoring.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateAntiForgeryTokenOnAllPostsAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            try
            {

                if (request.HttpMethod == WebRequestMethods.Http.Post)
                {
                    if (request.IsAjaxRequest())
                    {
                        var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                        var cookieValue = antiForgeryCookie != null
                            ? antiForgeryCookie.Value
                            : null;

                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    else
                    {
                        new ValidateAntiForgeryTokenAttribute()
                            .OnAuthorization(filterContext);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}