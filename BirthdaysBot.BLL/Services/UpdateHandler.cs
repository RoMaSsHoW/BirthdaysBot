using BirthdaysBot.BLL.Commands;
using BirthdaysBot.BLL.Helpers;
using Telegram.Bot.Types;

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
            var message = update.Message;
            var callbackQuery = update.CallbackQuery;
            var chatId = message?.Chat?.Id ?? callbackQuery!.Message!.Chat.Id;

            var userState = _stateMachine.GetUserState(chatId);

            if (message?.Chat == null && callbackQuery == null)
            {
                return;
            }

            if (message != null && message.Text!.Contains(CommandNames.StartEnC))
            {
                await ExecuteCommand(CommandNames.StartEnC, update);
                _stateMachine.SetUserState(chatId, UserState.MainMenu);
                return;
            }

            if (message != null && message.Text!.Contains(CommandNames.AddBirthdayRuC))
            {
                await ExecuteCommand(CommandNames.AddBirthdayRuC, update);
                _stateMachine.SetUserState(chatId, UserState.AddingBirthday);
                return;
            }

            if (message != null || callbackQuery != null)
            {
                switch (userState)
                {
                    case UserState.AddingBirthday:
                        await ExecuteCommand(CommandNames.AddBirthdayRuC, update);
                        break;
                }

            }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            var command = _commands.FirstOrDefault(c => c.CommandName == commandName);
            await command.ExecuteAsync(update);
        }
    }
}
