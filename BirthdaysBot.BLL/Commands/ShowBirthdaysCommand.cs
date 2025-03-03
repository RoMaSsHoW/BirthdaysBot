using System.Text;

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

            // Получаем список дней рождения (пока используем тестовые данные)
            var birthdays = new TestData().TestBirthdays; // Заменить на _birthdayService.GetBirthdays(user.Id);

            if (!birthdays.Any())
            {
                await _botClient.SendMessage(chatId.Value, "Список дней рождения пуст.");
                return;
            }

            // Формируем сообщение
            var messageText = new StringBuilder();
            foreach (var birthday in birthdays)
            {
                messageText.AppendLine($"{birthday.BirthdayId} {birthday.BirthdayName}");
                messageText.AppendLine($" Дата: {birthday.BirthdayDate:dd.MM.yyyy}");
                messageText.AppendLine($" TelegramUsername: {birthday.BirthdayTelegramUsername ?? "-"}");
                messageText.AppendLine();
            }

            // Отправляем сообщение
            await _botClient.SendMessage(chatId.Value, messageText.ToString().Trim());
        }
    }
}
