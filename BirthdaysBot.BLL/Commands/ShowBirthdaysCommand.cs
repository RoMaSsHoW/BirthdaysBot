namespace BirthdaysBot.BLL.Commands
{
    public class ShowBirthdaysCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;
        private readonly IBirthdayRepository _birthdayRepository;

        public ShowBirthdaysCommand(ITelegramBotClient botClient, IUserService userService, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _userService = userService;
            _birthdayRepository = birthdayRepository;
        }

        public override string CommandName => CommandNames.ShowBirthdaysRuC;

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

            var birthdays = await _birthdayRepository.GetBirthdaysAsync(chatId.Value);
            if (!birthdays.Any())
            {
                await _botClient.SendMessage(chatId.Value, "Список дней рождения пуст.");
                return;
            }

            var messageText = new StringBuilder();
            foreach (var birthday in birthdays)
            {
                messageText.AppendLine($"*{birthday.BirthdayName}");
                messageText.AppendLine($"  Дата: {birthday.BirthdayDate:dd.MM}");
                messageText.AppendLine($"  TelegramUsername: {birthday.BirthdayTelegramUsername ?? "-"}");
                messageText.AppendLine();
            }

            await _botClient.SendMessage(chatId.Value, messageText.ToString().Trim());
            StateMachine.ResetUserState(chatId.Value);
        }
    }
}
