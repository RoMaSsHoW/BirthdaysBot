using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Models;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    internal class BirthdayHandle : IHandleStrategy
    {
        public async Task Handle(ITelegramBotClient botClient, Update update, long chatId, UserBirthdayInfo state)
        {
            var messageText = update.Message?.Text;

            if (!IsValidDate(messageText, out DateTime birthdayDate))
            {
                await botClient.SendMessage(chatId, "Введите корректную дату в формате дд.мм:");
                return;
            }

            state.Birthday = birthdayDate;

            await botClient.SendMessage(chatId, "Хотите добавить Telegram Username?", replyMarkup: InlineButtons.AddOrSkipUsername);
        }

        private bool IsValidDate(string? inputDate, out DateTime birthdayDate)
        {
            return DateTime.TryParseExact(inputDate + ".2000", "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthdayDate);
        }
    }
}
