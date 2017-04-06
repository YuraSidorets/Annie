using OpenQA.Selenium.Remote;
using PMBot.Models;

namespace PMBot.BotServices
{
    public static class BotService
    {
        public static RemoteWebDriver Browser { get; set; }
        public static string Access_token { get; set; }

        public static LongPollServerResponse LongPollServer { get; set; }

        //    static VkApi vk = new VkApi();
        //    static BotLogic BotLogic = new BotLogic();

        //    public static void Authorize(ulong appID, string email, string pass, string token)
        //    {
        //        vk.Authorize(new ApiAuthParams
        //        {
        //            AccessToken = token,
        //            ApplicationId = 5962477, //appID,
        //            Login = email,
        //            Password = pass,
        //            Settings = Settings.All
        //        });
        //    }
        //    public static IEnumerable<Message> GetMessages()
        //    {
        //        return vk.Messages.Get(new MessagesGetParams()
        //        {
        //            Filters = MessagesFilter.All,
        //            Out = MessageType.Received,
        //        }).Messages;
        //    }

        //    public static void ProcessMessages()
        //    {
        //        LongPoolWatcher watcher = new LongPoolWatcher(vk);
        //        LongPollServerResponse pollServer = null;
        //        try
        //        {
        //            pollServer = vk.Messages.GetLongPollServer(true, true);

        //        }
        //        catch (Exception e)
        //        {
        //            throw e ;
        //        }
        //        watcher.StartAsync(pollServer.Ts, pollServer.Pts);
        //        watcher.NewMessages += Watcher_NewMessages;
        //    }

        //    private static void Watcher_NewMessages(VkApi owner, ReadOnlyCollection<Message> messages)
        //    {
        //        foreach (var message in messages)
        //        {
        //            BotLogic.Reply(vk,message, 5); // 5 for Moie Uvogenie 
        //        }
        //    }

        public static void GetLongPollServer()
        {
            var getLongPollServerScript =   "var xhr = new XMLHttpRequest();" + 
                                           $"xhr.open('GET', 'https://api.vk.com/method/messages.getLongPollServer?need_pts=1&access_token={Access_token}v=5.63', true);" +
                                            "xhr.onreadystatechange = function() {" + 
                                                                     "var req = new XMLHttpRequest();" +
                                                                     "var json = JSON.stringify(xhr.responseText);" +
                                                                         "req.open('POST', 'http://pmbot20170404120542.azurewebsites.net/api/vk/setLongPollServer', true);" +
                                                                         "req.setRequestHeader('Content-type', 'text/plain; charset=utf-8');"+
                                                                         "req.send(json);"+
                                                              "};"+
                                            "xhr.onerror = function() {" +
                                                                     "var req = new XMLHttpRequest();" +
                                                                     "var json = JSON.stringify(xhr.responseText);" +
                                                                         "req.open('POST', 'http://pmbot20170404120542.azurewebsites.net/api/vk/setLongPollServer', true);" +
                                                                         "req.setRequestHeader('Content-type', 'text/plain; charset=utf-8');" +
                                                                         "req.send(json);" +
                                                              "};" +
                                            "xhr.send();";

            Browser.ExecuteScript(getLongPollServerScript);
        }

    }
}