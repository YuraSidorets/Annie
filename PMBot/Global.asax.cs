using System;
using System.Threading.Tasks;
using PMBot.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah;
using PMBot.BotServices;

namespace PMBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LaunchSeq();
        }

        void Start()
        {
            var config = ConfigHelper.GetConfig();
            var controller = new VkController(VkService.GetInstance(config.Login, config.Pass, config.AppId, config.Phone, new Uri(config.WebDriver)));
            Task.Factory.StartNew(() => controller.StartLongPoolWatch());
        }

        void LaunchSeq()
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                LaunchSeq();
            }
        }
    }
}
