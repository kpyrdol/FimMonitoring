using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FIMMonitoring.Web.Startup))]
namespace FIMMonitoring.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
