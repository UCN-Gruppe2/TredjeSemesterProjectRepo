using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBookingInterface.Startup))]
namespace WebBookingInterface
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
