using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    public class HandleContext
    {
        private IHandleStrategy? _strategy;
        private readonly Update _update;
        private readonly long _chatId;

        public HandleContext(Update update, long chatId)
        {
            _update = update;
            _chatId = chatId;
        }

        public void SetStrategy(IHandleStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null.");
        }

        public async Task UseHandleAsync()
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Notification strategy must be set before sending a message.");
            }

            await _strategy.Handle(_update, _chatId);
        }
    }
}
