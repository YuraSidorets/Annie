using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMBot.Models
{
    public class MessagesSendParams
    {
        public int ChatId { get; set; }
        public string Message { get; set; }
    }
}