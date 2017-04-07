using System;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;

namespace PMBot.Helpers
{
    public class RemoteAuthControl
    {
        private string Email { get; set; }
        private string Pass { get; set; }
        private string AppID { get; set; }
        private string Phone { get; set; }
        public RemoteWebDriver Browser { get; }

        public RemoteAuthControl(string email,string pass, string appID, string phone, Uri browserUri)
        {
            Email = email;
            Pass = pass;
            AppID = appID;
            Phone = phone;
            Browser = new RemoteWebDriver(browserUri, DesiredCapabilities.PhantomJS());
        }
        public string Login()
        {
            Browser.Navigate().GoToUrl($"https://oauth.vk.com/authorize?client_id={AppID}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=messages&response_type=token&v=5.63");

            var userNameField = Browser.FindElementByName("email");
            var userPasswordField = Browser.FindElementByName("pass");
            var loginButton = Browser.FindElementById("install_allow");

            userNameField.SendKeys(Email);
            userPasswordField.SendKeys(Pass);

            var title = Browser.Title;
            loginButton.Click();
            //var url = Browser.Url;
            //Browser.Navigate().GoToUrl(url);


            if (!Browser.Url.Contains("access_token"))
            {
                try
                {
                    //If vk proceed phone number check
                    var numberNameField = Browser.FindElementByName("code");
                    numberNameField.SendKeys(TrimNumberForUraine(Phone));
                    loginButton = Browser.FindElementByClassName("button");
                    loginButton.Click();
                }
                catch (Exception e)
                {
                    //If phone number check doesn't need
                }
            }
            var access_token = string.Empty;
            if (Browser.Url.IndexOf("access_token", StringComparison.Ordinal) != -1)
            {
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(Browser.Url))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        access_token = m.Groups["value"].Value;
                       // BotServices.BotService.Authorize(0, "", "", m.Groups["value"].Value);
                        // Task.Run(() => BotServices.BotService.ProcessMessages());
                    }
                }
            }
            return access_token;
        }

        private string TrimNumberForUraine(string number)
        {
            return number.Remove(number.Length - 2, 2).Replace("380", "");
        }
    }
}