using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Models;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    internal class FullNameHandle : IHandleStrategy
    {
        public async Task Handle(ITelegramBotClient botClient, Update update, long chatId, UserBirthdayInfo state)
        {
            var messageText = update.Message?.Text;

            if (string.IsNullOrWhiteSpace(messageText) || !IsValidFullName(messageText) || !IsValidMessage(messageText))
            {
                await botClient.SendMessage(chatId, "Введите корректное ФИ (например: Иванов Иван)");
                return;
            }

            state.FullName = CapitalizeWords(messageText);

            await botClient.SendMessage(chatId, "Введите дату рождения в формате дд.мм:");
        }

        private bool IsValidMessage(string messageText)
        {
            if (messageText == CommandNames.AddBirthdayRuC || messageText == CommandNames.ShowBirthdaysRuC)
            {
                return false;
            }
            return true;
        }

        private bool IsValidFullName(string fullName)
        {
            return fullName.Trim().Split(' ').Length == 2;
        }

        private string CapitalizeWords(string input)
        {
            return string.Join(" ", input.Split(' ')
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        }
    }
}
