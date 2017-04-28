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
            RemoteAuthControl authControl = new RemoteAuthControl("", "", "", "", new Uri(""));
            BotService service = new BotService();
            BotService.AccessToken = authControl.Login();
            BotService.Browser = authControl.Browser;
            service.ProcessMessages(0);
        }
    }
}