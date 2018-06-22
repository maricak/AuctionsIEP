using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auctions.Web.Startup))]
namespace Auctions.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
