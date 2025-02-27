using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Models;
using BirthdaysBot.BLL.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Commands
{
    public class AddBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;

        private static readonly Dictionary<long, UserBirthdayInfo> _commandState = new();

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

            //if (!_commandState.ContainsKey(chatId.Value))
            //{
            //    //_commandState[chatId.Value] = new UserBirthdayInfo();
            //    //await _botClient.SendMessage(chatId.Value, "Введите ФИ (например: Иванов Иван)");
            //    //return;
            //}

            //if (update.Type == UpdateType.CallbackQuery)
            //{
            //    await HandleCallbackQuery(update.CallbackQuery, chatId.Value);
            //}
            //else if (update.Type == UpdateType.Message)
            //{
            //    await ProcessInput(chatId.Value, update.Message.Text, update.Message.MessageId);
            //}
        }

        //private async Task ProcessInput(long chatId, string messageText, int userMessageId)
        //{
        //    var state = _commandState[chatId];

        //    await Helper.DeleteMessage(_botClient, chatId, userMessageId);

        //    if (string.IsNullOrEmpty(state.FullName))
        //    {
        //        await HandleFullNameInput(chatId, messageText);
        //    }
        //    else if (state.Birthday == DateTime.MinValue)
        //    {
        //        await HandleBirthdayInput(chatId, messageText);
        //    }
        //    else if (string.IsNullOrEmpty(state.TelegramUsername))
        //    {
        //        await HandleUsernameInput(chatId, messageText);
        //    }
        //}

        //private async Task HandleFullNameInput(long chatId, string messageText)
        //{
        //    //if (string.IsNullOrWhiteSpace(messageText) || !IsValidFullName(messageText))
        //    //{
        //    //    await _botClient.SendMessage(chatId, "Введите корректное ФИ (например: Иванов Иван)");
        //    //    return;
        //    //}

        //    //_commandState[chatId].FullName = CapitalizeWords(messageText);
        //    //await _botClient.SendMessage(chatId, "Введите дату рождения в формате дд.мм:");
        //}

        //private async Task HandleBirthdayInput(long chatId, string messageText)
        //{
        //    if (!IsValidDate(messageText, out DateTime birthdayDate))
        //    {
        //        await _botClient.SendMessage(chatId, "Введите корректную дату в формате дд.мм:");
        //        return;
        //    }

        //    _commandState[chatId].Birthday = birthdayDate;
        //    await _botClient.SendMessage(chatId, "Хотите добавить Telegram Username?", replyMarkup: InlineButtons.AddOrSkipUsername);
        //}

        private async Task HandleUsernameInput(long chatId, string messageText)
        {
            _commandState[chatId].TelegramUsername = messageText ?? string.Empty;

            var userInfo = _commandState[chatId];
            await _botClient.SendMessage(chatId, $"Человек успешно добавлен!\n\n" +
                $"ФИ: {userInfo.FullName}\n" +
                $"Дата рождения: {userInfo.Birthday:dd.MM}\n" +
                $"Telegram Username: {userInfo.TelegramUsername}");

            _commandState.Remove(chatId);
        }

        private async Task HandleCallbackQuery(CallbackQuery callbackQuery, long chatId)
        {
            if (callbackQuery.Data == CommandNames.CallbackSkipUsernameC)
            {
                await HandleUsernameInput(chatId, null);
            }
            else
            {
                await _botClient.SendMessage(chatId, "Введите Telegram Username (например: @Oleg)");
            }
        }

        //private string CapitalizeWords(string input)
        //{
        //    return string.Join(" ", input.Split(' ')
        //        .Where(w => !string.IsNullOrWhiteSpace(w))
        //        .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        //}

        //private bool IsValidFullName(string fullName)
        //{
        //    return fullName.Trim().Split(' ').Length == 2;
        //}

        //private bool IsValidDate(string inputDate, out DateTime birthdayDate)
        //{
        //    return DateTime.TryParseExact(inputDate + ".2000", "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthdayDate);
        //}
    }
}
