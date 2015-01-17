using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (WebAPI.Backend.Startup))]

namespace WebAPI.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}