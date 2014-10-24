using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReferencingSystem.Application.Admin.MVC.Startup))]
namespace ReferencingSystem.Application.Admin.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
