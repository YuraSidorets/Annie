using PMBot.Models;
using System;

namespace PMBot.Helpers
{
    public class BotLogic
    {
        public void Reply(Message message, int chatId, Action<MessagesSendParams> Callback)
        {
           // if(message.ChatId != 5) { return;}
            if (message.Body.ToLower().StartsWith(@"/hi"))
            {
                Callback(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "Hi i'm head of PMO of Moe Uvogenie, nice to meet you guys."
                });
            }
            if (message.Body.ToLower().StartsWith(@"/help"))
            {
                Callback(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "i'm still in development, so it would be great if you have some ideas to improve me.\n funcs: / anya razberi poleti\n / anya razbudi vanyu\n "
                });
            }

            if (message.Body.ToLower().Contains(@"/anya razberi poleti"))
            {
                RazborPoldcastHelper poldcastHelper = new RazborPoldcastHelper();
                var lastRazbor = poldcastHelper.CheckRss();
                Callback(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "Hi, here is the last post on the Razbor Poletov:\n" + lastRazbor
                });
            }

            if (message.Body.ToLower().Contains(@"/anya razbudi vanyu"))
            {
                TelegramHelper telegramHelper = new TelegramHelper();
                telegramHelper.SendMessage("Vanya you need in vk");
                Callback(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "I will message Vanya on telegram\n"
                });
            }
        }
    }
}