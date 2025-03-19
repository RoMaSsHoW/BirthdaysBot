namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    public class HandleContext
    {
        private readonly ITelegramBotClient _botClient;
        private readonly Update _update;
        private readonly IBirthdayRepository _birthdayRepository;
        private static readonly Dictionary<long, UserBirthdayInfo> _state = new();

        public HandleContext(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository)
        {
            _botClient = botClient;
            _update = update;
            _birthdayRepository = birthdayRepository;
        }

        public async Task UseHandleAsync(long chatId)
        {
            var state = await GetOrCreateStateAsync(chatId);

            if (state == null) return;

            if (_update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackAsync(chatId, state);
            }
            else if (_update.Type == UpdateType.Message)
            {
                await StartOrContinueHandlingAsync(chatId, state);
            }

            if (state.IsComplete)
            {
                await CompleteRegistrationAsync(chatId, state);
            }
        }

        private async Task<UserBirthdayInfo> GetOrCreateStateAsync(long chatId)
        {
            if (!_state.ContainsKey(chatId))
            {
                _state[chatId] = new UserBirthdayInfo();
                await _botClient.SendMessage(chatId, "Введите ФИ (например: Иванов Иван)");
                return null;
            }
            return _state[chatId];
        }

        private async Task HandleCallbackAsync(long chatId, UserBirthdayInfo state)
        {
            var callbackData = _update.CallbackQuery?.Data;

            if (callbackData == CommandNames.CallbackAddUsernameC)
            {
                await _botClient.SendMessage(chatId, "Введите Telegram Username (например: @Oleg)");
            }
            else if (callbackData == CommandNames.CallbackSkipUsernameC)
            {
                await StartOrContinueHandlingAsync(chatId, state);
            }
        }

        private async Task StartOrContinueHandlingAsync(long chatId, UserBirthdayInfo state)
        {
            var strategy = SelectStrategy(state);
            if (strategy != null)
            {
                await strategy.Handle(_botClient, _update, chatId, state);
            }
        }

        private async Task CompleteRegistrationAsync(long chatId, UserBirthdayInfo state)
        {
            var result = await _birthdayRepository.CreateBirthdayAsync(state, chatId);

            var message = result ? $"✅ Человек успешно добавлен!" : Messages.ErrorMessage;

            await _botClient.SendMessage(chatId, message);

            _state.Remove(chatId);
            StateMachine.ResetUserState(chatId);
        }

        private IHandleStrategy SelectStrategy(UserBirthdayInfo state)
        {
            if (string.IsNullOrEmpty(state.FullName))
            {
                return new FullNameHandle();
            }
            else if (state.Birthday == DateOnly.MinValue)
            {
                return new BirthdayHandle();
            }
            else if (string.IsNullOrEmpty(state.TelegramUsername))
            {
                return new TelegramUsernameHandle();
            }
            else
            {
                throw new InvalidOperationException("Все данные уже заполнены или состояние некорректно.");
            }
        }
    }
}