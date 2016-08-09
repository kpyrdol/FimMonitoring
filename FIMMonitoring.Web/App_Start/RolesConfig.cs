using System;
using System.Web.Mvc;

namespace FIMMonitoring.Web
{
    [Flags]
    public enum ApplicationRoles
    {
        Customer = 1,
        Consultant = 2,
        Analyst = 4,
        Administrator = 8,
        CustomerLight = 16,
        TplUser = 32
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute : AuthorizeAttribute
    {
        public ApplicationRoles AppRoles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AppRoles != 0)
                Roles = AppRoles.ToString();

            base.OnAuthorization(filterContext);
        }
    }
}
