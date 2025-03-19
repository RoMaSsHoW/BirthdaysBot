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
                    await _botClient.SendMessage(chatId, $"Привет {message.Chat.FirstName}.\nДобро пожаловать в бота!", replyMarkup: ReplyButtons.MainKeybord);
                }
            }
            else
            {
                await _botClient.SendMessage(chatId, Messages.BadUser);
            }
        }
    }
}