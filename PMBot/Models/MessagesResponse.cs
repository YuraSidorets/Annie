﻿using Newtonsoft.Json;
using System.Collections.Generic;

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
        public int UserId { get; set; }

        [JsonProperty("read_state")]
        public int ReadState { get; set; }

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