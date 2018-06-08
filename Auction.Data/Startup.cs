using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auction.Data.Startup))]
namespace Auction.Data
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
