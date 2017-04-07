using PMBot.BotServices;
using System;
namespace PMBot.Helpers
{
    public static class BotStarter
    {
        public static void Start()
        {
            try
            {
                RemoteAuthControl authControl = new RemoteAuthControl("email", "pass", "appID", "phone", new Uri("https://phantomjs-1.herokuapp.com"));
                BotService.Access_token = authControl.Login();
                BotService.Browser = authControl.Browser;
                BotService.ProcessMessages(0);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}