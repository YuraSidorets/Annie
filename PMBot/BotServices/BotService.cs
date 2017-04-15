using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using PMBot.Helpers;
using PMBot.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PMBot.BotServices
{
    public static class BotService
    {
        public static RemoteWebDriver Browser { get; set; }
        private static LongPollServerResponse _pollServer = new LongPollServerResponse();
        public static string AccessToken { get; set; }
        private static bool _firstrun = true;

        /// <summary>
        /// Start Message Processing
        /// </summary>
        /// <param name="ts">Ts parameter from poll history response, don't need if first run </param>
        public static void ProcessMessages(int ts)
        {
            Config config = new Config();
            try
            {

               var configString = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"));
                config = JsonConvert.DeserializeObject<Config>(configString);
            }
            catch (Exception e)
            {
                System.IO.File.CreateText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"));
                System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"),
                                   JsonConvert.SerializeObject(new Config(){ChatId = "5",Assemblies = new List<string>(){"Test.dll"},ServiceUrl = "http://localhost/"}, Formatting.Indented));
            }

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

            if (config != null)
            {
                chat = int.Parse(config.ChatId);
            }

            foreach (var update in history.Updates)
            {
                if (Convert.ToString(update[0]).Equals("4"))  //4 for new message in chat
                {
                    var actualChatId = Convert.ToInt64(update[3]) - 2000000000; //3 for chat id
                    if (actualChatId != chat)
                    {
                        continue;
                    }
                    var message = Convert.ToString(update[6]);
                    if (Convert.ToString(update[6]).Contains("/")) //6 for text of the message
                    {
                        try
                        {
                            foreach (var assembly in config.Assemblies)
                            {
                                HttpClient client = new HttpClient();
                                var res = RunLogicAsync(client, new Parameter { AssemblyName = assembly, InvokeParameter = message },config.ServiceUrl);
                                SendMessage(new MessagesSendParams { ChatId = chat, Message = res });
                            }
                        }
                        catch
                        {
                            SendMessage(new MessagesSendParams { ChatId = chat, Message = "я упаалаа :с" });
                        }
                        // _botLogic.Reply(new Message { Body = message, ChatId = chat }, chat, SendMessage);
                    }
                }
            }
            ProcessMessages(history.Ts);
        }

        static string RunLogicAsync(HttpClient client, Parameter parameter,string url)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url+ "/api/Plugin/Run", parameter).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            };
            return response.Content.ReadAsStringAsync().Result;
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