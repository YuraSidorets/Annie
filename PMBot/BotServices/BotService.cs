using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using PMBot.Helpers;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNetExtend;

namespace PMBot.BotServices
{
    public static class BotService
    {
        static VkApi vk = new VkApi();
        static BotLogic BotLogic = new BotLogic();

        public static void Authorize(ulong appID, string email, string pass, string token)
        {
            vk.Authorize(new ApiAuthParams
            {
                AccessToken = token,
                ApplicationId = , //appID,
                Login = email,
                Password = pass,
                Settings = Settings.All
            });
        }
        public static IEnumerable<Message> GetMessages()
        {
            return vk.Messages.Get(new MessagesGetParams()
            {
                Filters = MessagesFilter.All,
                Out = MessageType.Received,
            }).Messages;
        }

        public static void ProcessMessages()
        {
            LongPoolWatcher watcher = new LongPoolWatcher(vk);
            LongPollServerResponse pollServer = null;
            try
            {
                pollServer = vk.Messages.GetLongPollServer(true, true);

            }
            catch (Exception e)
            {
                throw e ;
            }
            watcher.StartAsync(pollServer.Ts, pollServer.Pts);
            watcher.NewMessages += Watcher_NewMessages;
        }

        private static void Watcher_NewMessages(VkApi owner, ReadOnlyCollection<Message> messages)
        {
            foreach (var message in messages)
            {
                BotLogic.Reply(vk,message, 5); // 5 for Moie Uvogenie 
            }
        }
    }
}