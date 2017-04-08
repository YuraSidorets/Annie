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
            RemoteAuthControl authControl = new RemoteAuthControl("email", "pass", "appID", "phone", new Uri("remote selenium web driver url"));
            BotService.AccessToken = authControl.Login();
            BotService.Browser = authControl.Browser;
            BotService.ProcessMessages(0);
        }
    }
}