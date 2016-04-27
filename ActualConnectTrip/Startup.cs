using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ActualConnectTrip.Startup))]
namespace ActualConnectTrip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
