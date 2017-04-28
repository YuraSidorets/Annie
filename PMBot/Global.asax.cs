using System.Threading.Tasks;
using PMBot.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah;

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
            Task.Factory.StartNew(BotStarter.Start);
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
