using Newtonsoft.Json;
using System.Collections.Generic;

namespace PMBot.Models
{
    public class LongPollServerHistory
    {

        [JsonProperty("ts")]
        public int Ts { get; set; }

        [JsonProperty("updates")]
        public IList<IList<object>> Updates { get; set; }

    }
}