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
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            if (chatId == null) return;

            var user = _userService.GetUser(update);
            if (user == null)
            {
                await _botClient.SendMessage(chatId.Value, Messages.BadUser);
                StateMachine.ResetUserState(chatId.Value);
                return;
            }

            var mainKeybord = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { CommandNames.AddBirthdayRuC, CommandNames.ShowBirthdaysRuC},
                new KeyboardButton[] { CommandNames.DeleteBirthdayRuC/*, CommandNames.UpdateBirthdayRuC*/ }
            })
            {
                ResizeKeyboard = true // Клавиатура будет адаптирована под экран
            };

            var message = update.Message;
            if (message != null)
            {
                await _botClient.SendMessage(chatId.Value, $"Привет {message.Chat.FirstName}.\nДобро пожаловать в бота!", replyMarkup: mainKeybord);
            }
        }
    }
}