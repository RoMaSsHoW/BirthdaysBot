using BirthdaysBot.BLL.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    internal class TelegramUsernameHandle : IHandleStrategy
    {
        public async Task Handle(ITelegramBotClient botClient, Update update, long chatId, UserBirthdayInfo state)
        {
            var messageText = update.Message?.Text;

            state.TelegramUsername = messageText ?? "-";
        }
    }
}
