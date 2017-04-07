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
        public static string Access_token { get; set; }
        static BotLogic BotLogic = new BotLogic();
        public static LongPollServerResponse LongPollServer { get; set; }
        static bool Firstrun = true;
        static LongPollServerResponse pollServer = new LongPollServerResponse();

        public static void ProcessMessages(int ts)
        {
            if (Firstrun == true)
            {
                pollServer = GetLongPollServer();
                Firstrun = false;
            }
            else
            {
                pollServer.Response.Ts = ts;
            }

            var history = GetLongPollHistory(pollServer);
            int chat = 5;
            string messagesIds = "";
            foreach(var update in history.Updates)
            {
                if (Convert.ToString(update[0]).Equals("4"))
                {
                    messagesIds += update[1] + ",";
                }
            }
            if (messagesIds != "")
            {
                var messages = GetMessagesById(messagesIds.TrimEnd(','));
                foreach (var message in messages.Response.Items)
                {
                    if (message.Body.Contains("/"))
                    {
                        BotLogic.Reply(new Message { Body = message.Body, ChatId = chat }, chat, SendMessage);
                    }
                }
                ProcessMessages(history.Ts);
            }
            else
            {
                ProcessMessages(history.Ts);
            }
        }

        public static LongPollServerResponse GetLongPollServer()
        {
            Browser.Url = $"https://api.vk.com/method/messages.getLongPollServer?need_pts=1&access_token={Access_token}&v=5.63";
            var response = Browser.PageSource;
            response = ClearResponse(response);
            return JsonConvert.DeserializeObject<LongPollServerResponse>(response);
        }

        public static LongPollServerHistory GetLongPollHistory(LongPollServerResponse server)
        {
            Browser.Url = $"https://{server.Response.Server}?act=a_check&key={server.Response.Key}&ts={server.Response.Ts}&wait=5&mode=2&version=1";
            var response = Browser.PageSource;
            response = ClearResponse(response);
            return JsonConvert.DeserializeObject<LongPollServerHistory>(response, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
                    });
        }

        private static string ClearResponse(string response)
        {
            response = response.Remove(0, response.IndexOf('{'));
            return response.Remove(response.LastIndexOf('}') + 1, response.Length - response.LastIndexOf('}') - 1);
        }

        private static void SendMessage(MessagesSendParams param)
        {
            Browser.Url = $"https://api.vk.com/method/messages.send?chat_id={param.ChatId}&message={param.Message}&access_token={Access_token}&v=5.63";            
        }

        private static MessagesResponse GetMessagesById(string ids)
        {
            Browser.Url = $"https://api.vk.com/method/messages.getById?message_ids={ids}&access_token={Access_token}&v=5.63";
            var response = Browser.PageSource;
            response = ClearResponse(response);
            return JsonConvert.DeserializeObject<MessagesResponse>(response);
        }

    }
}