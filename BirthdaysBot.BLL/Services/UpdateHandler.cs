namespace BirthdaysBot.BLL.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly List<BaseCommand> _commands;
        private readonly IUserService _userService;

        public UpdateHandler(IEnumerable<BaseCommand> commands, IUserService userService)
        {
            _commands = commands.ToList();
            _userService = userService;
        }

        public async Task Execute(Update update)
        {
            // Получаем данные из обновления
            var message = update.Message;
            var callbackQuery = update.CallbackQuery;

            // Проверяем, что обновление содержит данные
            if (message == null && callbackQuery == null)
            {
                return;
            }

            var chatId = message?.Chat?.Id ?? callbackQuery!.Message!.Chat.Id;
            var userState = StateMachine.GetUserState(chatId);

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
                    StateMachine.SetUserState(chatId, UserState.AddingBirthday);
                    return;
                }

                // Обработка команды "Показать др"
                if (message.Text.Contains(CommandNames.ShowBirthdaysRuC))
                {
                    await ExecuteCommand(CommandNames.ShowBirthdaysRuC, update);
                    StateMachine.SetUserState(chatId, UserState.GettingBirthday);
                    return;
                }

                // Обработка команды "Удалить др"
                if (message.Text.Contains(CommandNames.DeleteBirthdayRuC))
                {
                    await ExecuteCommand(CommandNames.DeleteBirthdayRuC, update);
                    StateMachine.SetUserState(chatId, UserState.DeletingBirthday);
                    return;
                }

                // Обработка команды "Редактировать др"
                //if (message.Text.Contains(CommandNames.UpdateBirthdayRuC))
                //{
                //    await ExecuteCommand(CommandNames.UpdateBirthdayRuC, update);
                //    _stateMachine.SetUserState(chatId, UserState.UpdatingBirthday);
                //    return;
                //}
            }

            if (message != null || callbackQuery != null)
            {
                if (userState != UserState.MainMenu)
                {
                    string commandName = GetCommandName(userState);

                    await ExecuteCommand(commandName, update);
                }
            }
        }

        private string GetCommandName(UserState userState)
        {
            return userState switch
            {
                UserState.AddingBirthday => CommandNames.AddBirthdayRuC,
                UserState.DeletingBirthday => CommandNames.DeleteBirthdayRuC,
                //UserState.UpdatingBirthday => CommandNames.UpdateBirthdayRuC,
                _ => throw new ArgumentException("Invalid user state")
            };
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