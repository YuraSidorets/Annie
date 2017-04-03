using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace PMBot.Helpers
{
    public class BotLogic
    {
        public void Reply(VkApi vkApi, Message message, int chatId)
        {
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

        }
    }
}