namespace BirthdaysBot.BLL.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly List<BaseCommand> _commands;
        private readonly StateMachine _stateMachine;
        private readonly IUserService _userService;

        public UpdateHandler(IEnumerable<BaseCommand> commands, StateMachine stateMachine, IUserService userService)
        {
            _commands = commands.ToList();
            _stateMachine = stateMachine;
            _userService = userService;
        }

        public async Task Execute(Update update)
        {
            // Получаем данные из обновления
            var message = update.Message;
            var callbackQuery = update.CallbackQuery;

            // Проверяем, что обновление содержит данные
            if (message?.Chat == null && callbackQuery == null)
            {
                return;
            }

            var chatId = message?.Chat?.Id ?? callbackQuery!.Message!.Chat.Id;
            var userState = _stateMachine.GetUserState(chatId);

            // Обработка команд
            if (message?.Text != null)
            {
                // Обработка команды /start
                if (message.Text.Contains(CommandNames.StartEnC))
                {
                    await ExecuteCommand(CommandNames.StartEnC, update);
                    return;
                }

                // Обработка команды "Добавить др"
                if (message.Text.Contains(CommandNames.AddBirthdayRuC))
                {
                    await ExecuteCommand(CommandNames.AddBirthdayRuC, update);
                    _stateMachine.SetUserState(chatId, UserState.AddingBirthday);
                    return;
                }

                // Обработка команды "Показать др"
                if (message.Text.Contains(CommandNames.ShowBirthdaysRuC))
                {
                    await ExecuteCommand(CommandNames.ShowBirthdaysRuC, update);
                    _stateMachine.SetUserState(chatId, UserState.GettingBirthday);
                    return;
                }

                // Обработка команды "Удалить др"
                if (message.Text.Contains(CommandNames.DeleteBirthdayRuC))
                {
                    await ExecuteCommand(CommandNames.DeleteBirthdayRuC, update);
                    _stateMachine.SetUserState(chatId, UserState.DeletingBirthday);
                    return;
                }
            }

            if (message != null || callbackQuery != null)
            {
                switch (userState)
                {
                    case UserState.AddingBirthday:
                        await ExecuteCommand(CommandNames.AddBirthdayRuC, update);
                        break;
                    case UserState.DeletingBirthday:
                        await ExecuteCommand(CommandNames.DeleteBirthdayRuC, update);
                        break;
                }

            }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            var command = _commands.FirstOrDefault(c => c.CommandName == commandName);
            if (command != null)
            {
                await command.ExecuteAsync(update);
            }
        }
    }
}