namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    internal class ShowBirthdaysListStrategy : IDeleteStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository, long chatId)
        {
            var birthdays = await birthdayRepository.GetBirthdaysAsync(chatId);

            if (!birthdays.Any())
            {
                await botClient.SendMessage(chatId, "Список дней рождения пуст.");
                return;
            }

            var buttons = birthdays
                .Select(birthday => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{birthday.BirthdayName} {birthday.BirthdayDate:dd.MM}",
                        $"{birthday.BirthdayId}")
                })
                .ToArray();

            InlineKeyboardMarkup birthdayForDelete = new(buttons);

            await botClient.SendMessage(chatId, "Выберите дату которую хотите удалить:", replyMarkup: birthdayForDelete);
        }
    }
}
