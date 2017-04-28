using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using PMBot.Models;

namespace PMBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _configPath = System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt");

        private Config GetConfig()
        {
           return JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(_configPath));
        }

        public ActionResult Index()
        {
            return View(GetConfig());
        }

        [System.Web.Http.HttpPost]
        public void SetChatId(string chatId)
        {
            var config = GetConfig();
            if (config != null)
            {
                Config tmpConfig = new Config { Assemblies = config.Assemblies, ChatId = chatId, ServiceUrl = config.ServiceUrl };
                System.IO.File.WriteAllText(_configPath,
                    JsonConvert.SerializeObject(tmpConfig, Formatting.Indented));
            }
        }

        [System.Web.Http.HttpPost]
        public void SetService(string serviceUrl)
        {
            var config = GetConfig();
            if (config != null)
            {
                Config tmpConfig = new Config { Assemblies = config.Assemblies, ChatId = config.ChatId, ServiceUrl = serviceUrl };
                System.IO.File.WriteAllText(_configPath,
                    JsonConvert.SerializeObject(tmpConfig, Formatting.Indented));
            }
        }

        [System.Web.Http.HttpPost]
        public void RegisterAssembly(List<string> files)
        {
            var config = GetConfig();
            if (config != null)
            {
                foreach (var file in files)
                {
                    if (!config.Assemblies.Contains(file))
                    {
                        config.Assemblies.Add(file);
                    }
                }
                System.IO.File.WriteAllText(_configPath,
    JsonConvert.SerializeObject(config, Formatting.Indented));

            }
        }

        [System.Web.Http.HttpPost]
        public void SendMessage([FromBody] string message)
        {
            var config = GetConfig();
            if (config != null &&  message != null)
            {
                var service = new BotServices.BotService();
                service.SendMessage(new MessagesSendParams { ChatId = int.Parse(config.ChatId), Message = message });
            }
        }
    }
}
