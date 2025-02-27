using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Services;
using BirthdaysBot.BLL.Services.Strategies.AddBirthday;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Commands
{
    public class AddBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;

        public AddBirthdayCommand(ITelegramBotClient botClient, IUserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        public override string CommandName => CommandNames.AddBirthdayRuC;

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

            var handleContext = new HandleContext(_botClient, update);

            await handleContext.UseHandleAsync(chatId.Value);
        }
    }
}

//private async Task HandleCallbackQuery(CallbackQuery callbackQuery, long chatId)
//{
//    if (callbackQuery.Data == CommandNames.CallbackSkipUsernameC)
//    {
//        await HandleUsernameInput(chatId, null);
//    }
//    else
//    {
//        await _botClient.SendMessage(chatId, "Введите Telegram Username (например: @Oleg)");
//    }
//}