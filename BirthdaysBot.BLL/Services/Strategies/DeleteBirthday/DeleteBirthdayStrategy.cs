namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    internal class DeleteBirthdayStrategy : IDeleteStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository, long chatId)
        {
            var callbackData = update.CallbackQuery?.Data;
            
            if (callbackData == null)
            {
                await botClient.SendMessage(chatId, Messages.ErrorMessage);
                return;
            }

            if (callbackData.StartsWith("delete_"))
            {
                // Если выбрана дата, спрашиваем подтверждение
                if (int.TryParse(callbackData.Replace("delete_", ""), out int birthdayId))
                {
                    var buttons = new InlineKeyboardMarkup(new[]
                    {
                    InlineKeyboardButton.WithCallbackData("✅ Да", $"confirm_delete_{birthdayId}"),
                    InlineKeyboardButton.WithCallbackData("❌ Нет", "cancel_delete")
                });

                    await botClient.SendMessage(chatId, "Вы уверены, что хотите удалить этот день рождения?", replyMarkup: buttons);
                }
                else
                {
                    await botClient.SendMessage(chatId, Messages.ErrorMessage);
                }
            }
            else if (callbackData.StartsWith("confirm_delete_"))
            {
                // Если пришло подтверждение удаления
                if (int.TryParse(callbackData.Replace("confirm_delete_", ""), out int birthdayId))
                {
                    var result = await birthdayRepository.DeleteBirthdayAsync(birthdayId, chatId);
                    if (result)
                    {
                        await botClient.SendMessage(chatId, "✅ День рождения успешно удален!");
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
            else if (callbackData == "cancel_delete")
            {
                // Если пользователь отменил удаление
                await botClient.SendMessage(chatId, "Удаление отменено.");
            }
        }
    }
}
