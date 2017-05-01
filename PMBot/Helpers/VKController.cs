using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Elmah;
using Newtonsoft.Json;
using PMBot.BotServices;
using PMBot.Models;

namespace PMBot.Helpers
{
    public class VKController
    {
        public BotService API { get;}

        public LongPoolWatcher LongPool { get; private set; }

        public event MessagesRecievedDelegate NewMessages;

        public VKController(BotService api = null)
        {
            if (api == null)
                API = BotService.GetInstance(email, pass, appID, phone, webDriver); 
            else
                API = api;
        }

        public void StartLongPoolWatch(int? LastTs = null, int? LastPts = null)
        {
            if (LongPool == null)
            {
                LongPool = new LongPoolWatcher(API);
                LongPool.NewMessages += LongPool_NewMessages;
            }
            LongPool.StartAsync(LastTs, LastPts);
        }

        #region Events
        private void LongPool_NewMessages(BotService owner, IList<IList<object>> messages)
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

        private readonly string _configPath = System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt");

        /// <summary>
        /// Start Message Processing
        /// </summary>
        public void ProcessMessages(BotService service, IList<IList<object>> messages)
        {
            Config config = new Config();
            try
            {
                var configString = System.IO.File.ReadAllText(_configPath);
                config = JsonConvert.DeserializeObject<Config>(configString);
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                System.IO.File.CreateText(_configPath);
                System.IO.File.WriteAllText(_configPath, JsonConvert.SerializeObject(new Config { ChatId = "5", Assemblies = new List<string>() { "Test.dll" }, ServiceUrl = "http://localhost/" }, Formatting.Indented));
            }
            int chat = 5; //Chat ID

            if (config != null)
            {
                chat = int.Parse(config.ChatId);
            }

            foreach (var update in messages)
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
                            foreach (var assembly in config.Assemblies.Distinct())
                            {
                                HttpClient client = new HttpClient();
                                var res = RunLogicAsync(client, new Parameter { AssemblyName = assembly, InvokeParameter = message }, config.ServiceUrl);
                                service.SendMessage(new MessagesSendParams { ChatId = chat, Message = res });
                            }
                        }
                        catch (Exception e)
                        {
                            ErrorSignal.FromCurrentContext().Raise(e);
                            service.SendMessage(new MessagesSendParams { ChatId = chat, Message = "я упаалаа :с" });
                        }
                     }
                }
            }
        }

        private string RunLogicAsync(HttpClient client, Parameter parameter, string url)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + "/api/Plugin/Run", parameter).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Run bot logic failure");
            };
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
