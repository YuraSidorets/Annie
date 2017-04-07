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
            

            return new EmptyResult();
        }
    }
}
