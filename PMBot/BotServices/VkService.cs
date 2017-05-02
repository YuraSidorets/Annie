using System;
using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using PMBot.Helpers;
using PMBot.Models;

namespace PMBot.BotServices
{
    public class VkService
    {
        private static VkService _instance;
        private static object _syncRoot = new Object();

        public static RemoteWebDriver Browser { get; set; }
        public static string AccessToken { get; set; }

        public static VkService GetInstance(string login, string pass, string appId, string phone, Uri remoteWebDriver)
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new VkService(login, pass, appId, phone, remoteWebDriver);
                }
            }
            return _instance;
        }

        protected VkService(string login, string pass, string appId, string phone, Uri remoteWebDriver)
        {
            if (Browser == null || AccessToken == null)
            {
                RemoteAuthControl authControl = new RemoteAuthControl(login, pass, appId, phone, remoteWebDriver);
                AccessToken = authControl.Login();
                Browser = authControl.Browser;
            }
        }

        /// <summary>
        /// Get VK Long Poll Server
        /// </summary>
        /// <returns>LongPollServerResponse</returns>
        public LongPollServerResponse GetLongPollServer()
        {
            return JsonConvert.DeserializeObject<LongPollServerResponse>(GetResponse($"https://api.vk.com/method/messages.getLongPollServer?need_pts=1&access_token={AccessToken}&v=5.63"));
        }

        /// <summary>
        /// Get VK Long Poll History
        /// </summary>
        /// <param name="server">Long Poll Server</param>
        /// <returns>LongPollServerHistory response</returns>
        public  LongPollServerHistory GetLongPollHistory(LongPollServerResponse server)
        {
            return JsonConvert.DeserializeObject<LongPollServerHistory>(GetResponse($"https://{server.Response.Server}?act=a_check&key={server.Response.Key}&ts={server.Response.Ts}&wait=5&mode=2&version=1"), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Send Message to chat
        /// </summary>
        /// <param name="param">Message Parameters</param>
        public void SendMessage(MessagesSendParams param)
        {
            Browser.Url = $"https://api.vk.com/method/messages.send?chat_id={param.ChatId}&message={param.Message}&access_token={AccessToken}&v=5.63";
        }

        /// <summary>
        /// Clear response from HTML from remote browser 
        /// </summary>
        /// <param name="response">Response string</param>
        /// <returns>Cleared response json string</returns>
        private string ClearResponse(string response)
        {
            response = response.Remove(0, response.IndexOf('{'));
            return response.Remove(response.LastIndexOf('}') + 1, response.Length - response.LastIndexOf('}') - 1);
        }
        
        /// <summary>
        /// Get response from remote browser
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Response string</returns>
        private string GetResponse(string url)
        {
            Browser.Url = url;
            return ClearResponse(Browser.PageSource);
        }
    }
}