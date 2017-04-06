using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace PMBot.Helpers
{
    public class BotLogic
    {
        public void Reply(VkApi vkApi, Message message, int chatId)
        {
            if(message.ChatId != 5) { return;}
            if (message.Body.ToLower().StartsWith(@"\hi"))
            {
                vkApi.Messages.Send(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "Hi i'm head of PMO of Moe Uvogenie, nice to meet you guys."
                });
            }
            if (message.Body.ToLower().StartsWith(@"\help"))
            {
                vkApi.Messages.Send(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "i'm still in development, so it would be great if you have some ideas to improve me."
                });
            }

            if (message.Body.ToLower().Contains(@"\anya razberi poleti"))
            {
                RazborPoldcastHelper poldcastHelper = new RazborPoldcastHelper();
                var lastRazbor = poldcastHelper.CheckRss();
                vkApi.Messages.Send(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "Hi, here is the last post on the Razbor Poletov:\n" + lastRazbor
                });
            }

            if (message.Body.ToLower().Contains(@"\anya razbudi vanyu"))
            {
             //  TelegramHelper.SendMessage();
                vkApi.Messages.Send(new MessagesSendParams()
                {
                    ChatId = chatId,
                    Message = "I will message Vanya on telegram\n"
                });
            }
        }
    }
}