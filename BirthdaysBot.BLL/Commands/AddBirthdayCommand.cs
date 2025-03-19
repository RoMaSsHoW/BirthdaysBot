namespace BirthdaysBot.BLL.Commands
{
    public class AddBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;
        private readonly IBirthdayRepository _birthdayRepository;

        public AddBirthdayCommand(ITelegramBotClient botClient, IUserRepository userRepository, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _userRepository = userRepository;
            _birthdayRepository = birthdayRepository;
        }

        public override string CommandName => CommandNames.AddBirthdayRuC;

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

            var handleContext = new HandleContext(_botClient, update, _birthdayRepository);

            await handleContext.UseHandleAsync(chatId.Value);
        }
    }
}