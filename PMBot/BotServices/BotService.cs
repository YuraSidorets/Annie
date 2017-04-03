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
    public class BotService
    {
        VkApi vk = new VkApi();
        BotLogic BotLogic = new BotLogic();

        public void Authorize(ulong appID, string email, string pass)
        {
            vk.Authorize(new ApiAuthParams
            {
                ApplicationId = appID,
                Login = email,
                Password = pass,
                Settings = Settings.All
            });
        }

        public IEnumerable<Message> GetMessages()
        {
            return vk.Messages.Get(new MessagesGetParams()
            {
                Filters = MessagesFilter.All,
                Out = MessageType.Received,
            }).Messages;
        }

        public LongPollServerResponse GetLongPollServer()
        {
            return vk.Messages.GetLongPollServer(true, true);
        }

        public void ProcessMessages()
        {
            LongPoolWatcher watcher = new LongPoolWatcher(vk);
            var pollServer = GetLongPollServer();

            watcher.StartAsync(pollServer.Ts, pollServer.Pts);
            watcher.NewMessages += Watcher_NewMessages;
        }

        private void Watcher_NewMessages(VkApi owner, ReadOnlyCollection<Message> messages)
        {
            foreach (var message in messages)
            {
                BotLogic.Reply(vk,message,5); // 5 for Moie Uvogenie
            }
        }
    }
}