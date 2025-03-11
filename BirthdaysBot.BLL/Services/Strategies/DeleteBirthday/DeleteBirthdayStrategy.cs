namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    internal class DeleteBirthdayStrategy : IDeleteStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository, long chatId)
        {
            var callbackData = update.CallbackQuery?.Data;

            if (callbackData != null && int.TryParse(callbackData, out int birthdayId))
            {
                var result = await birthdayRepository.DeleteBirthdayAsync(birthdayId, chatId);
                if (result)
                {
                    await botClient.SendMessage(chatId, $"День рождения успешно удален!");
                }
                else
                {
                    await botClient.SendMessage(chatId, Messages.ErrorMessage);
                }
            }
            else
            {
                await botClient.SendMessage(chatId, Messages.ErrorMessage);
            }
        }
    }
}
