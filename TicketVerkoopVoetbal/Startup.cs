using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TicketVerkoopVoetbal.Startup))]
namespace TicketVerkoopVoetbal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
