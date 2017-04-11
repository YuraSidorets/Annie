using PMBot.BotServices;
using System;
namespace PMBot.Helpers
{
    public static class BotStarter
    {
        /// <summary>
        /// Start bot
        /// </summary>
        public static void Start()
        {
            RemoteAuthControl authControl = new RemoteAuthControl("380967656138", "LetTheDevilIn", "5962477", "380967656138", new Uri("http://phantomjs-1.herokuapp.com"));
            BotService.AccessToken = authControl.Login();
            BotService.Browser = authControl.Browser;
            BotService.ProcessMessages(0);
        }
    }
}