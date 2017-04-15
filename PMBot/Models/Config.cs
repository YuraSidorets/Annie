using System.Collections.Generic;

namespace PMBot.Models
{
    public class Config
    {
        public string ServiceUrl { get; set; }
        public string ChatId { get; set; }
        public IList<string> Assemblies { get; set; }
    }
}