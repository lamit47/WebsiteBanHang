using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GearBatOn.Startup))]
namespace GearBatOn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
