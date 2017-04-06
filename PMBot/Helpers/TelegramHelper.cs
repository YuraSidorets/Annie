using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace PMBot.Helpers
{
    public static class TelegramHelper
    {

        public static async void SendMessage()
        {
            var Bot = new TelegramBotClient("");
            await Bot.SendTextMessageAsync("-138805831", "Vanya Wake Up");
        }
    }
}