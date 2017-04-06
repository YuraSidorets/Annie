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

        public static async void SendMessage(string message)
        {
            var Bot = new TelegramBotClient("250719418:AAEmTZ1OSFGOD38UaMhHfQvy47_6MYhYcds");
            await Bot.SendTextMessageAsync("-138805831", message);
        }
    }
}