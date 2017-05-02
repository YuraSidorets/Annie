using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Elmah;
using PMBot.BotServices;
using PMBot.Models;

namespace PMBot.Helpers
{
    public class VkController
    {
        public VkService Api { get; }
        public LongPoolWatcher LongPool { get; private set; }
        public event MessagesRecievedDelegate NewMessages;

        public VkController(VkService api = null)
        {
            if (api == null)
            {
                var config = ConfigHelper.GetConfig();
                Api = VkService.GetInstance(config.Login, config.Pass, config.AppId, config.Phone, new Uri(config.WebDriver));
            }
            else
                Api = api;
        }

        public void StartLongPoolWatch(int? lastTs = null, int? lastPts = null)
        {
            if (LongPool == null)
            {
                LongPool = new LongPoolWatcher(Api);
                LongPool.NewMessages += LongPool_NewMessages;
            }
            LongPool.StartAsync(lastTs, lastPts);
        }

        #region Events
        private void LongPool_NewMessages(VkService owner, IList<IList<object>> messages)
        {
            NewMessages?.Invoke(owner, messages);
            try
            {
                ProcessMessages(owner, messages);
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }

        }
        #endregion

        /// <summary>
        /// Start Message Processing
        /// </summary>
        public void ProcessMessages(VkService service, IList<IList<object>> messages)
        {
            var config = ConfigHelper.GetConfig();

            int chatId = 5; //Chat ID
            if (config != null)
                chatId = int.Parse(config.ChatId);

            foreach (var update in messages)
            {
                if (!Convert.ToString(update[0]).Equals("4")) continue;
                if (!Convert.ToString(update[6]).Contains("/")) continue;
                var message = Convert.ToString(update[6]);
                try
                {
                    if (config != null)
                        foreach (var assembly in config.Assemblies.Distinct())
                        {
                            HttpClient client = new HttpClient();
                            var res = RunLogicAsync(client, new Parameter { AssemblyName = assembly, InvokeParameter = message }, config.ServiceUrl);

                            var actualChatId = Convert.ToInt64(update[3]) - 2000000000; //3 for chat id
                            if (actualChatId != chatId) continue;
                            service.SendMessage(new MessagesSendParams { ChatId = chatId, Message = res });
                        }
                }
                catch (Exception e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    service.SendMessage(new MessagesSendParams { ChatId = chatId, Message = "down" });
                }
            }
        }

        private string RunLogicAsync(HttpClient client, Parameter parameter, string url)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + "/api/Plugin/Run", parameter).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new Exception("Run bot logic failure");
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
