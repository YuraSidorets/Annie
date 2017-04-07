using Newtonsoft.Json;

namespace PMBot.Models
{
    public class LongPollServerResponse
    {

        [JsonProperty("response")]
        public ServerHistoryResponse Response { get; set; }
    }

    public class ServerHistoryResponse
    {

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("ts")]
        public int Ts { get; set; }

        [JsonProperty("pts")]
        public int Pts { get; set; }
    }

}