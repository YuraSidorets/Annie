using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using PMBot.Models;

namespace PMBot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Config config = new Config();
                var configString = System.IO.File.ReadAllText(Server.MapPath("~\\config.txt"));
                config = JsonConvert.DeserializeObject<Config>(configString);
            return View(config);
        }

        [System.Web.Http.HttpPost]
        public void SetChatId( string chatId)
        {
            var configString = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"));
            var config = JsonConvert.DeserializeObject<Config>(configString);
            if (config != null)
            {
                Config tmpConfig = new Config {Assemblies = config.Assemblies, ChatId = chatId, ServiceUrl = config.ServiceUrl};
                System.IO.File.WriteAllText(Server.MapPath("~\\config.txt"),
                    JsonConvert.SerializeObject(tmpConfig, Formatting.Indented));
            }
        }

        [System.Web.Http.HttpPost]
        public void SetService(string serviceUrl)
        {
            var configString = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"));
            var config = JsonConvert.DeserializeObject<Config>(configString);
            if (config != null)
            {
                Config tmpConfig = new Config { Assemblies = config.Assemblies, ChatId = config.ChatId, ServiceUrl = serviceUrl };
                System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"),
                    JsonConvert.SerializeObject(tmpConfig, Formatting.Indented));
            }
        }

        [System.Web.Http.HttpPost]
        public void RegisterAssembly(List<string> files)
        {
            var configString = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt"));
            var config = JsonConvert.DeserializeObject<Config>(configString);
            if (config != null)
            {
                foreach (var file in files)
                {
                    config.Assemblies.Add(file);
                }
            }
        }
    }
}
