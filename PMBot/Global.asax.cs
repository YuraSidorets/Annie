using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OpenQA.Selenium.Remote;
using PMBot.BotServices;

namespace PMBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //BotService service = new BotService();
            //service.Authorize(0, , );
            //Task.Run(() => service.ProcessMessages());
            Run();
        }

        public void Run()
        {
            var browser = new RemoteWebDriver(new Uri(""), DesiredCapabilities.PhantomJS());
            browser.Navigate().GoToUrl("https://oauth.vk.com/authorize?client_id=&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=messages&response_type=token&v=5.63");

            var userNameField = browser.FindElementByName("email");
            var userPasswordField = browser.FindElementByName("pass");
            var loginButton = browser.FindElementById("install_allow");

            userNameField.SendKeys("");
            userPasswordField.SendKeys("");
            var title = browser.Title;
            loginButton.Click();
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var url = browser.Url;
            browser.Navigate().GoToUrl(url);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            try
            {
                var numberNameField = browser.FindElementByName("code");
                numberNameField.SendKeys("");
                loginButton = browser.FindElementByClassName("button");
                loginButton.Click();
            }
            catch (Exception e)
            {
            }
            if (browser.Url.IndexOf("access_token", StringComparison.Ordinal) != -1)
            {
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(browser.Url))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        BotServices.BotService.Authorize(0, "", "", m.Groups["value"].Value);
                        Task.Run(() => BotServices.BotService.ProcessMessages());
                    }
                }
                return;
            }
        }
    }
}
