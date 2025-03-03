namespace BirthdaysBot.BLL.Commands
{
    public class ShowBirthdaysCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;

        public ShowBirthdaysCommand(ITelegramBotClient botClient, IUserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        public override string CommandName => CommandNames.ShowBirthdaysRuC;

        public override async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            if (chatId == null)
            {
                return;
            }

            var user = _userService.GetUser(update);
            if (user == null)
            {
                await _botClient.SendMessage(chatId.Value, Messages.BadUser);
                return;
            }
        }
    }
}
