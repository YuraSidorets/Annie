using Telegram.Bot;

namespace PMBot.Helpers
{
    public class TelegramHelper
    {
        /// <summary>
        /// Message to Telegram using Bot
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            var bot = new TelegramBotClient("");
            var res = bot.SendTextMessageAsync("-138805831", message).Result;
        }
    }
}