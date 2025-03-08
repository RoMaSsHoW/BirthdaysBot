namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    internal class BirthdayHandle : IHandleStrategy
    {
        public async Task Handle(ITelegramBotClient botClient, Update update, long chatId, UserBirthdayInfo state)
        {
            var messageText = update.Message?.Text;

            if (!IsValidDate(messageText, out DateOnly birthdayDate))
            {
                await botClient.SendMessage(chatId, "Введите корректную дату в формате дд.мм:");
                return;
            }

            state.Birthday = birthdayDate;

            await botClient.SendMessage(chatId, "Хотите добавить Telegram Username?", replyMarkup: InlineButtons.AddOrSkipUsername);
        }

        private bool IsValidDate(string? inputDate, out DateOnly birthdayDate)
        {
            return DateOnly.TryParseExact(inputDate + ".2000", "dd.MM.yyyy", out birthdayDate);
        }
    }
}
