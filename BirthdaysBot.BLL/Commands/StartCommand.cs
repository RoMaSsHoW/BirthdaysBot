using BirthdaysBot.BLL.Buttons;
using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;

        public StartCommand(ITelegramBotClient botClient, IUserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        public override string CommandName => CommandNames.StartEnC;

        public override async Task ExecuteAsync(Update update)
        {
            var user = _userService.GetUser(update);
            var message = update.Message;
            var chatId = message.Chat.Id;
            if (user != null)
            {
                if (message != null)
                {
                    await _botClient.SendMessage(chatId, $"Привет {message.Chat.Username}.\nДобро пожаловать в бота!\nВаш chatId - {chatId}", replyMarkup: ReplyButtons.MainKeybord);
                }
            }
            else
            {
                await _botClient.SendMessage(chatId, Messages.BadUser);
            }
        }
    }
}