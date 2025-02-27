using BirthdaysBot.BLL.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    public class HandleContext
    {
        private readonly Update _update;
        private readonly ITelegramBotClient _botClient;
        private IHandleStrategy? _strategy;
        private static readonly Dictionary<long, UserBirthdayInfo> _state = new();

        public HandleContext(ITelegramBotClient botClient, Update update)
        {
            _botClient = botClient;
            _update = update;
        }

        public async Task UseHandleAsync(long chatId)
        {
            if (!_state.ContainsKey(chatId))
            {
                _state[chatId] = new UserBirthdayInfo();
                await _botClient.SendMessage(chatId, "Введите ФИ (например: Иванов Иван)");
                return;
            }

            var state = _state[chatId];

            _strategy = SelectStrategy(state);

            await _strategy.Handle(_botClient, _update, chatId, state);
        }

        private IHandleStrategy SelectStrategy(UserBirthdayInfo state)
        {
            if (string.IsNullOrEmpty(state.FullName))
            {
                return new FullNameHandle();
            }
            else if (state.Birthday == DateTime.MinValue)
            {
                return new BirthdayHandle();
            }
            else if (string.IsNullOrEmpty(state.TelegramUsername))
            {
            }
            return new FullNameHandle();
        }

        private void SetStrategy(IHandleStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null.");
        }
    }
}







//if (_strategy == null)
//{
//    throw new InvalidOperationException("Notification strategy must be set before sending a message.");
//}

//await _strategy.Handle(_update, _chatId);

