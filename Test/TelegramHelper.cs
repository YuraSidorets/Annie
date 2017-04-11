using Telegram.Bot;

namespace BotLogic
{
    public class TelegramHelper
    {
        /// <summary>
        /// Message to Telegram using Bot
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            var bot = new TelegramBotClient("250719418:AAEmTZ1OSFGOD38UaMhHfQvy47_6MYhYcds");
            var res = bot.SendTextMessageAsync("-138805831", message).Result;
        }
    }
}