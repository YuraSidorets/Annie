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
            var controller =
                new VKController(BotService.GetInstance(email, pass, appID, phone, webDriver)));
            Task.Factory.StartNew(() => controller.StartLongPoolWatch());
        }

        void LaunchSeq()
        {
            try
            {
                Start();
            }
            catch (System.Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                LaunchSeq();
            }
        }
    }
}
