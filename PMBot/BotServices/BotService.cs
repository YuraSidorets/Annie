using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using PMBot.Helpers;
using PMBot.Models;
using System;

namespace PMBot.BotServices
{
    public static class BotService
    {
        public static RemoteWebDriver Browser { get; set; }
        private static BotLogic _botLogic = new BotLogic();
        private static LongPollServerResponse _pollServer = new LongPollServerResponse();
        public static string AccessToken { get; set; }
        private static bool _firstrun = true;

        /// <summary>
        /// Start Message Processing
        /// </summary>
        /// <param name="ts">Ts parameter from poll history response, don't need if first run </param>
        public static void ProcessMessages(int ts)
        {
            if (_firstrun)
            {
                _pollServer = GetLongPollServer();
                _firstrun = false;
            }
            else
            {
                _pollServer.Response.Ts = ts;
            }

            var history = GetLongPollHistory(_pollServer);
            int chat = 5; //Chat ID

            foreach (var update in history.Updates)
            {
                if (Convert.ToString(update[0]).Equals("4"))  //4 for new message in chat
                {
                    var actualChatId = 2000000000 - Convert.ToInt64(update[3]); //3 for chat id
                    if (actualChatId != chat)
                    {
                        continue;
                    }
                    var message = Convert.ToString(update[6]);
                    if (Convert.ToString(update[6]).Contains("/")) //6 for text of the message
                    {
                        _botLogic.Reply(new Message { Body = message, ChatId = chat }, chat, SendMessage);
                    }
                }
            }
            ProcessMessages(history.Ts);
        }

        /// <summary>
        /// Get VK Long Poll Server
        /// </summary>
        /// <returns>LongPollServerResponse</returns>
        public static LongPollServerResponse GetLongPollServer()
        {
            return JsonConvert.DeserializeObject<LongPollServerResponse>(GetResponse($"https://api.vk.com/method/messages.getLongPollServer?need_pts=1&access_token={AccessToken}&v=5.63"));
        }

        /// <summary>
        /// Get VK Long Poll History
        /// </summary>
        /// <param name="server">Long Poll Server</param>
        /// <returns>LongPollServerHistory response</returns>
        public static LongPollServerHistory GetLongPollHistory(LongPollServerResponse server)
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
        private static void SendMessage(MessagesSendParams param)
        {
            Browser.Url = $"https://api.vk.com/method/messages.send?chat_id={param.ChatId}&message={param.Message}&access_token={AccessToken}&v=5.63";
        }

        /// <summary>
        /// Clear response from HTML from remote browser 
        /// </summary>
        /// <param name="response">Response string</param>
        /// <returns>Cleared response json string</returns>
        private static string ClearResponse(string response)
        {
            response = response.Remove(0, response.IndexOf('{'));
            return response.Remove(response.LastIndexOf('}') + 1, response.Length - response.LastIndexOf('}') - 1);
        }
        
        /// <summary>
        /// Get response from remote browser
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Response string</returns>
        private static string GetResponse(string url)
        {
            Browser.Url = url;
            return ClearResponse(Browser.PageSource);
        }
    }
}