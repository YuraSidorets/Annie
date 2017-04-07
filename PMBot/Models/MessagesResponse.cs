using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMBot.Models
{
    public class MessagesResponse
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public class Item
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("out")]
        public int Out { get; set; }

        [JsonProperty("user_id")]
        public int User_id { get; set; }

        [JsonProperty("read_state")]
        public int Read_state { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class Response
    {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }
    }

}