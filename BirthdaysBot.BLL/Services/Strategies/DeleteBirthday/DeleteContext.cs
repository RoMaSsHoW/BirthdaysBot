namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    public class DeleteContext
    {
        private readonly Update _update;
        private readonly ITelegramBotClient _botClient;
        private readonly IBirthdayRepository _birthdayRepository;
        private IDeleteStrategy _deleteStrategy;

        public DeleteContext(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository)
        {
            _update = update;
            _botClient = botClient;
            _birthdayRepository = birthdayRepository;
        }

        public void SetStrategy(IDeleteStrategy strategy)
        {
            _deleteStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Стратегия не может быть null.");
        }

        public async Task ExecuteStrategyAsync(long chatId)
        {
            if (_deleteStrategy == null)
            {
                throw new InvalidOperationException("Стратегия должна быть установлена перед её выполнением.");
            }
            await _deleteStrategy.ExecuteAsync(_botClient, _update, _birthdayRepository, chatId);
        }
    }
}
