namespace BirthdaysBot.BLL.Commands
{
    public class UpdateBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;
        private readonly IBirthdayRepository _birthdayRepository;

        public UpdateBirthdayCommand(ITelegramBotClient botClient, IUserService userService, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _userService = userService;
            _birthdayRepository = birthdayRepository;
        }

        public override string CommandName => CommandNames.UpdateBirthdayRuC;

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

            var messageText = update.Message?.Text;
            var callbackData = update.CallbackQuery?.Data;

            if (update.Type == UpdateType.Message)
            {
                if (messageText.Contains(CommandNames.UpdateBirthdayRuC))
                {
                    await ShowBirthdays(chatId.Value);
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {

            }
        }

        private async Task ShowBirthdays(long chatId)
        {
            var birthdays = await _birthdayRepository.GetBirthdaysAsync(chatId);

            if (!birthdays.Any())
            {
                await _botClient.SendMessage(chatId, "Список дней рождения пуст.");
                return;
            }

            var buttons = birthdays
                .Select(birthday => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{birthday.BirthdayName} {birthday.BirthdayDate:dd.MM}",
                        $"update_{birthday.BirthdayId}")
                })
                .ToArray();

            InlineKeyboardMarkup birthdayForDelete = new(buttons);

            await _botClient.SendMessage(chatId, "Выберите дату которую хотите удалить:", replyMarkup: birthdayForDelete);
        }

        private async Task SelectFieldForUpdating(Update update, long chatId)
        {
            var callbackData = update.CallbackQuery?.Data;
        }
    }
}
