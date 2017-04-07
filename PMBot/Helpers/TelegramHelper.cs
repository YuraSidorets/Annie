using Telegram.Bot;

namespace PMBot.Helpers
{
    public class TelegramHelper
    {

        public void SendMessage(string message)
        {
            var Bot = new TelegramBotClient("");
            var res = Bot.SendTextMessageAsync("-138805831", message).Result;
        }
    }
}