using Newtonsoft.Json;
using PMBot.Models;

namespace PMBot.Helpers
{
    public static class ConfigHelper
    {
        private static readonly string ConfigPath = System.Web.Hosting.HostingEnvironment.MapPath("~\\config.txt");

        public static Config GetConfig()
        {
            return JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(ConfigPath));
        }

        public static void SetConfig(Config config)
        {
            System.IO.File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }
}