namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    internal class DeleteBirthdayStrategy : IDeleteStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository, long chatId)
        {
            var callbackData = update.CallbackQuery?.Data;

            if (string.IsNullOrEmpty(callbackData))
            {
                await SendMessageAndResetState(botClient, chatId, Messages.ErrorMessage);
                return;
            }

            if (callbackData.StartsWith("delete_"))
            {
                await HandleDeleteRequest(botClient, chatId, callbackData);
            }
            else if (callbackData.StartsWith("confirm_delete_"))
            {

            }
            else if (callbackData == "cancel_delete")
            {
                await SendMessageAndResetState(botClient, chatId, "Удаление отменено.");
            }
        }

        private async Task HandleDeleteRequest(ITelegramBotClient botClient, long chatId, string callbackData)
        {
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
                await SendMessageAndResetState(botClient, chatId, Messages.ErrorMessage);
            }
        }

        private async Task HandleDeleteConfirm(ITelegramBotClient botClient, IBirthdayRepository birthdayRepository, long chatId, string callbackData)
        {
            if (int.TryParse(callbackData.Replace("confirm_delete_", ""), out int birthdayId))
            {
                var result = await birthdayRepository.DeleteBirthdayAsync(birthdayId, chatId);
                var message = result ? $"✅ День рождения успешно удален!" : Messages.ErrorMessage;
                await SendMessageAndResetState(botClient, chatId, message);
            }
            else
            {
                await SendMessageAndResetState(botClient, chatId, Messages.ErrorMessage);
            }
        }

        private async Task SendMessageAndResetState(ITelegramBotClient botClient, long chatId, string message)
        {
            await botClient.SendMessage(chatId, message);
            StateMachine.ResetUserState(chatId);
        }
    }
}
