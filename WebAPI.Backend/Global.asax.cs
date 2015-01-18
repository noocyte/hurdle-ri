using System.Web.Http;
using WebAPI.Backend.App_Start;

namespace WebAPI.Backend
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Configure();
        }
    }
}