namespace BirthdaysBot.BLL.Commands
{
    public class UpdateBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;
        private readonly IBirthdayRepository _birthdayRepository;

        public UpdateBirthdayCommand(ITelegramBotClient botClient, IUserRepository userRepository, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _userRepository = userRepository;
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

            var user = await _userRepository.UserExistsAsync(chatId.Value);
            if (!user)
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
                if (callbackData.StartsWith("update_"))
                {
                    await SelectFieldForUpdating(update, chatId.Value);
                }
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
            var callbackData = update.CallbackQuery!.Data!;

            if (!callbackData.StartsWith("update_")) return;

            if (int.TryParse(callbackData.Replace("update_", ""), out int birthdayId))
            {
                var birthday = await _birthdayRepository.GetBirthdayAsync(birthdayId, chatId);

                var buttons = new InlineKeyboardMarkup(new[]
                {
                    new[] { InlineKeyboardButton.WithCallbackData("Фамилия Имя", $"edit_name_{birthdayId}") },
                    new[] { InlineKeyboardButton.WithCallbackData("Дата рождения", $"edit_date_{birthdayId}") },
                    new[] { InlineKeyboardButton.WithCallbackData("Telegram Username", $"edit_username_{birthdayId}") }
                });

                var messageText = $"Фамилия Имя:  {birthday.BirthdayName}\n" +
                                  $"Дата рождения:  {birthday.BirthdayDate:dd.MM}\n" +
                                  $"Telegram Username:  {birthday.BirthdayTelegramUsername}\n" +
                                   "Что вы хотите отредактировать?";

                await _botClient.SendMessage(chatId, messageText, replyMarkup: buttons);
            }
        }
    }
}
