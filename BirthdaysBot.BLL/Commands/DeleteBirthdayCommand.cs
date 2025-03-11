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

            var birthdays = await _birthdayRepository.GetBirthdaysAsync(chatId.Value);

            if (!birthdays.Any())
            {
                await _botClient.SendMessage(chatId.Value, "Список дней рождения пуст.");
                return;
            }

            var buttons = birthdays
                .Select(birthday => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{birthday.BirthdayName} {birthday.BirthdayDate:dd.MM}",
                        $"{birthday.BirthdayId}")
                })
                .ToArray();

            InlineKeyboardMarkup birthdayForDelete = new(buttons);

            await _botClient.SendMessage(chatId.Value, "Выберите дату которую хотите удалить:", replyMarkup: birthdayForDelete);
        }
    }
}
