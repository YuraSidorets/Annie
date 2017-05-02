using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using Elmah;
using PMBot.BotServices;
using PMBot.Helpers;
using PMBot.Models;

namespace PMBot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(ConfigHelper.GetConfig());
        }

        [System.Web.Http.HttpPost]
        public void SetChatId(string chatId)
        {
            var config = ConfigHelper.GetConfig();
            if (config != null)
            {
                Config tmpConfig = new Config { Assemblies = config.Assemblies, ChatId = chatId, ServiceUrl = config.ServiceUrl };
                ConfigHelper.SetConfig(tmpConfig);
            }
        }

        [System.Web.Http.HttpPost]
        public void SetService(string serviceUrl)
        {
            var config = ConfigHelper.GetConfig();
            if (config != null)
            {
                Config tmpConfig = new Config { Assemblies = config.Assemblies, ChatId = config.ChatId, ServiceUrl = serviceUrl };
                ConfigHelper.SetConfig(tmpConfig);
            }
        }

        [System.Web.Http.HttpPost]
        public void RegisterAssembly(List<string> files)
        {
            var config = ConfigHelper.GetConfig();
            if (config != null)
            {
                foreach (var file in files)
                {
                    if (!config.Assemblies.Contains(file))
                    {
                        config.Assemblies.Add(file);
                    }
                }
                ConfigHelper.SetConfig(config);

            }
        }

        [System.Web.Http.HttpPost]
        public void SendMessage([FromBody] string message)
        {
            try
            {
                var config = ConfigHelper.GetConfig();
                if (config != null && message != null)
                {
                    VkService.GetInstance(config.Login, config.Pass, config.AppId, config.Phone, new Uri(config.WebDriver)).SendMessage(new MessagesSendParams { ChatId = int.Parse(config.ChatId), Message = message });
                }
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
        }
    }
}
