namespace BirthdaysBot.BLL.Commands
{
    public class DeleteBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;
        private readonly IBirthdayRepository _birthdayRepository;

        public DeleteBirthdayCommand(ITelegramBotClient botClient, IUserService userService, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _userService = userService;
            _birthdayRepository = birthdayRepository;
        }

        public override string CommandName => CommandNames.DeleteBirthdayRuC;

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

            var context = new DeleteContext(_botClient, update, _birthdayRepository);

            if (update.Type == UpdateType.Message)
            {
                context.SetStrategy(new ShowBirthdaysListStrategy());
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                context.SetStrategy(new DeleteBirthdayStrategy());
            }

            await context.ExecuteStrategyAsync(chatId.Value);
        }
    }
}