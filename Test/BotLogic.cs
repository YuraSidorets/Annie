namespace BotLogic
{
    public class MainBotLogic
    {
        public string Run(string message)
        {
            if (message.ToLower().StartsWith(@"/hi"))
            {
                return "Hi i'm head of PMO of Moe Uvogenie, nice to meet you guys.";
            }
            if (message.ToLower().StartsWith(@"/help"))
            {
                return "i'm still in development, so it would be great if you have some ideas to improve me.\n funcs: / anya razberi poleti\n / anya razbudi vanyu\n ";
            }
            if (message.ToLower().Contains(@"/anya razberi poleti"))
            {
                RazborPoldcastHelper poldcastHelper = new RazborPoldcastHelper();
                var lastRazbor = poldcastHelper.CheckRss();
                return "Hi, here is the last post on the Razbor Poletov:\n" + lastRazbor;
            }
            if (message.ToLower().Contains(@"/anya razbudi vanyu"))
            {
                TelegramHelper telegramHelper = new TelegramHelper();
                telegramHelper.SendMessage("Vanya you need in vk");
                return "I will message Vanya on telegram\n";
            }
            return "";
        }
    }
}
