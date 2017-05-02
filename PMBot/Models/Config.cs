using System.Collections.Generic;

namespace PMBot.Models
{
    public class Config
    {
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Phone { get; set; }
        public string AppId { get; set; }
        public string WebDriver { get; set; }

        public string ServiceUrl { get; set; }
        public string ChatId { get; set; }
        public IList<string> Assemblies { get; set; }

        public Config()
        {
            Login = "";
            Pass = "";
            Phone = "";
            AppId = "";
            WebDriver = "";
            ServiceUrl = "localhost";
            ChatId = "5";
            Assemblies = new List<string> { "Test.dll" };
        }
    }
}