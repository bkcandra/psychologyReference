using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReferencingSystem.Application.Main.MVC.Startup))]
namespace ReferencingSystem.Application.Main.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
