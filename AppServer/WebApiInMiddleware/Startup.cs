using Owin;
using System.Web.Http;

namespace WebApiInMiddleware
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration webApiConfiguration = ConfigureWebApi();
            app.UseWebApi(webApiConfiguration);
        }

        private HttpConfiguration ConfigureWebApi()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            return config;
        }
    }
}
