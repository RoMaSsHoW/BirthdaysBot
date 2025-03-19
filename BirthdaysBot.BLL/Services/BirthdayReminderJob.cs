namespace BirthdaysBot.BLL.Services
{
    public class BirthdayReminderJob : IJob
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBirthdayRepository _birthdayRepository;

        public BirthdayReminderJob(ITelegramBotClient botClient, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _birthdayRepository = birthdayRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var birthdays = await _birthdayRepository.GetAllBirthdaysAsync();

            var messages = birthdays
                .Where(b => b.BirthdayDate.HasValue &&
                            new DateOnly(today.Year, b.BirthdayDate.Value.Month, b.BirthdayDate.Value.Day) >= today &&
                            new DateOnly(today.Year, b.BirthdayDate.Value.Month, b.BirthdayDate.Value.Day) <= today.AddDays(4))
                .Select(b => new Tuple<long?, string>(b.UserChatId, GetBirthdayMessage(b, today)))
                .Where(tuple => !string.IsNullOrEmpty(tuple.Item2))
                .ToList();

            if (messages != null)
            {
                foreach (var tuple in messages)
                {
                    var chatId = tuple.Item1;
                    var message = tuple.Item2;

                    await _botClient.SendMessage(chatId!, message);
                }
            }
        }

        private string GetBirthdayMessage(BirthdayDTO birthday, DateOnly today)
        {
            if (!birthday.BirthdayDate.HasValue) return string.Empty;

            var birthdayWithCorrectYear = new DateOnly(today.Year, birthday.BirthdayDate.Value.Month, birthday.BirthdayDate.Value.Day);

            int daysLeft = (birthdayWithCorrectYear.DayNumber - today.DayNumber);

            var telegramUsername = birthday.BirthdayTelegramUsername == "-" ? string.Empty : birthday.BirthdayTelegramUsername;

            return daysLeft switch
            {
                4 => $"Через 3 дня у {birthday.BirthdayName} будет день рождения",
                3 => $"Через 2 дня у {birthday.BirthdayName} будет день рождения",
                2 => $"После завтра у {birthday.BirthdayName} будет день рождения",
                1 => $"Завтра у {birthday.BirthdayName} будет день рождения",
                0 => $"Сегодня у {birthday.BirthdayName} день рождения {telegramUsername}",
                _ => string.Empty
            };
        }
    }
}