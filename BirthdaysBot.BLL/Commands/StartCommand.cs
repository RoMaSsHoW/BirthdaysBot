namespace BirthdaysBot.BLL.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;

        public StartCommand(ITelegramBotClient botClient, IUserRepository userRepository)
        {
            _botClient = botClient;
            _userRepository = userRepository;
        }

        public override string CommandName => CommandNames.StartEnC;

        public override async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            if (chatId == null) return;

            var user = await _userRepository.UserExistsAsync(chatId.Value);
            if (!user)
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