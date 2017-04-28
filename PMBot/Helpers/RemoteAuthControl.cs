using System;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;
using Elmah;

namespace PMBot.Helpers
{
    public class RemoteAuthControl
    {
        private string Email { get; }
        private string Pass { get; }
        private string AppId { get; }
        private string Phone { get; }
        public  RemoteWebDriver Browser { get; }

        /// <summary>
        /// Set parameters to login VK
        /// </summary>
        /// <param name="email">Vk Email</param>
        /// <param name="pass">Vk Pass</param>
        /// <param name="appId">Vk App ID</param>
        /// <param name="phone">Vk phone</param>
        /// <param name="browserUri">Remote phantom JS Browser Url</param>
        public RemoteAuthControl(string email, string pass, string appId, string phone, Uri browserUri)
        {
            Email = email;
            Pass = pass;
            AppId = appId;
            Phone = phone;
            Browser = new RemoteWebDriver(browserUri, DesiredCapabilities.PhantomJS());
        }

        /// <summary>
        /// Login to Vk
        /// </summary>
        /// <returns>Access token string</returns>
        public string Login()
        {
            Browser.Navigate().GoToUrl($"https://oauth.vk.com/authorize?client_id={AppId}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=messages&response_type=token&v=5.63");

            var userNameField = Browser.FindElementByName("email");
            var userPasswordField = Browser.FindElementByName("pass");
            var loginButton = Browser.FindElementById("install_allow");

            userNameField.SendKeys(Email);
            userPasswordField.SendKeys(Pass);
            loginButton.Click();

            if (!Browser.Url.Contains("access_token"))
            {
                try
                {
                    //If vk proceed phone number check
                    var numberNameField = Browser.FindElementByName("code");
                    numberNameField.SendKeys(TrimNumberForUraine());
                    loginButton = Browser.FindElementByClassName("button");
                    loginButton.Click();
                }
                catch (Exception e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    //If phone number check doesn't need, to suppress possible errors
                }
            }
            var accessToken = string.Empty;
            if (Browser.Url.IndexOf("access_token", StringComparison.Ordinal) != -1)
            {
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(Browser.Url))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        accessToken = m.Groups["value"].Value;
                    }
                }
            }
            return accessToken;
        }

        /// <summary>
        /// If phone number check provided, trim phone number
        /// </summary>
        /// <returns>Trimmed string to pass number check</returns>
        private string TrimNumberForUraine()
        {
            return Phone.Remove(Phone.Length - 2, 2).Replace("380", "");
        }
    }
}