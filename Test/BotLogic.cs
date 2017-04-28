using System;

namespace BotLogic
{
    public class MainBotLogic
    {
        public string Run(string message)
        {
            message = message.ToLower();
            if (message.StartsWith(@"/hi"))
            {
                return "Hi i'm head of PMO of Moe Uvogenie, nice to meet you guys.";
            }
            if (message.StartsWith(@"/help"))
            {
                return "i'm still in development, so it would be great if you have some ideas to improve me.\n funcs: / anya razberi poleti\n / anya razbudi vanyu\n ";
            }
            if (message.Contains(@"/anya razberi poleti"))
            {
                var lastRazbor = RazborPoldcastHelper.CheckRss();
                return "Hi, here is the last post on the Razbor Poletov:\n" + lastRazbor;
            }
            if (message.Contains(@"/send "))
            {
                TelegramHelper telegramHelper = new TelegramHelper();
                var msg = message.Substring(message.IndexOf(" ", StringComparison.Ordinal));
                telegramHelper.SendMessage(msg);
                return "I will message Vanya on telegram\n";
            }
            return "";
        }
    }
}
