using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdaysBot.API.TestService
{
    public class ServiceTest
    {
        private readonly ITelegramBotClient _botClient;

        public ServiceTest(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                await HandleMessage(update.Message);
            }
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery?.Data != null)
            {
                await HandleCallbackQueryAsync(update.CallbackQuery);
            }
        }

        private async Task HandleMessage(Message message)
        {
            long chatId = message.Chat.Id;
            string text = message.Text;

            if (text == "/start")
            {
                await _botClient.SendTextMessageAsync(chatId, $"Привет {message.Chat.Username}");
            }
            else if (text == "/1")
            {
                var buttons = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1", "number1"),
                        InlineKeyboardButton.WithCallbackData("2", "number2")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("3", "number3")
                    }
                });

                await _botClient.SendTextMessageAsync(chatId, "Веберите действие:", replyMarkup: buttons);
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, $"Вы написали: {text}");
            }
        }

        private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
        {
            long chatId = callbackQuery.Message.Chat.Id;
            int botMessageId = callbackQuery.Message.MessageId;

            await _botClient.DeleteMessageAsync(chatId, botMessageId);

            switch (callbackQuery.Data)
            {
                case "number1":
                    await _botClient.SendTextMessageAsync(chatId, "Выбрано действие 1");
                    break;

                case "number2":
                    await _botClient.SendTextMessageAsync(chatId, "Выбрано действие 2");
                    break;
                case "number3":
                    await _botClient.SendTextMessageAsync(chatId, "Выбрано действие 3");
                    break;
            }
        }
    }
}