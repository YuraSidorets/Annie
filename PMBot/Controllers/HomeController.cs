using System;
using System.Web.Mvc;
using PMBot.Helpers;
using PMBot.BotServices;

namespace PMBot.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public EmptyResult Index()
        {
            try
            {
                RemoteAuthControl authControl = new RemoteAuthControl("380632169098", "LetTheDevilIn", "5962477", "380632169098", new Uri("https://phantomjs-1.herokuapp.com"));
                BotService.Access_token = authControl.Login();
                BotService.Browser = authControl.Browser;
                BotService.GetLongPollServer();
            }
            catch (Exception e)
            {
                throw e;
            }

            return new EmptyResult();
        }
    }
}
