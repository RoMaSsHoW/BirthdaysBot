using BirthdaysBot.BLL.Buttons;
using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Services;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BirthdaysBot.BLL.Commands
{
    public class AddBirthdayCommand : BaseCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;

        private Dictionary<long, InputStage> _commandState = new();

        private string fullName = string.Empty; // поменять
        private DateTime birthday = DateTime.MinValue;
        private string telegramUsername = string.Empty;

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

            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(update.CallbackQuery, chatId.Value);
            }
            
            if (update.Type == UpdateType.Message)
            {
                await HandleMessage(update.Message, chatId.Value);
            }

        }

        private async Task HandleMessage(Message message, long chatId)
        {
            var messageText = message.Text;

            InputStage currentState = _commandState.ContainsKey(chatId) ? _commandState[chatId] : InputStage.Start;

            switch (currentState)
            {
                case InputStage.Start:
                    _commandState[chatId] = InputStage.RequestFullName;
                    await RestartCommand(chatId);
                    break;
                case InputStage.RequestFullName:
                    await _botClient.SendMessage(chatId, "Введите ФИ (например: Иванов Иван)");
                    _commandState[chatId] = InputStage.AwaitingFullName;
                    break;
                case InputStage.AwaitingFullName:
                    if (!string.IsNullOrEmpty(messageText))
                    {
                        if (IsValidFullName(messageText))
                        {
                            fullName = CapitalizeWords(messageText);
                            _commandState[chatId] = InputStage.RequestBirthday;
                            await RestartCommand(chatId);
                        }
                        else
                        {
                            await _botClient.SendMessage(chatId, "Вы не корректно ввели фамилию и имя");
                            _commandState[chatId] = InputStage.RequestFullName;
                            await RestartCommand(chatId);
                        }
                    }
                    else
                    {
                        await _botClient.SendMessage(chatId, "Строка не должна быть пустой");
                        _commandState[chatId] = InputStage.RequestFullName;
                        await RestartCommand(chatId);
                    }
                    break;
                case InputStage.RequestBirthday:
                    await _botClient.SendMessage(chatId, "Введите дату рождения в формате дд.мм:");
                    _commandState[chatId] = InputStage.AwaitingBirthday;
                    break;
                case InputStage.AwaitingBirthday:
                    if (!string.IsNullOrEmpty(messageText))
                    {
                        if (IsValidDate(messageText, out DateTime birthdayDate))
                        {
                            birthday = birthdayDate;
                            _commandState[chatId] = InputStage.RequestUsernameChoice;
                            await RestartCommand(chatId);
                        }
                        else
                        {
                            await _botClient.SendMessage(chatId, "Вы не корректно ввели дату");
                            _commandState[chatId] = InputStage.RequestBirthday;
                            await RestartCommand(chatId);
                        }
                    }
                    else
                    {
                        await _botClient.SendMessage(chatId, "Строка не должна быть пустой");
                        _commandState[chatId] = InputStage.RequestBirthday;
                        await RestartCommand(chatId);
                    }
                    break;
                case InputStage.RequestUsernameChoice:
                    await _botClient.SendMessage(chatId, "Хотите добавить Telegram Username этого человека?", replyMarkup: InlineButtons.AddOrSkipUsername);
                    break;
                case InputStage.RequestTelegramUsername:
                    await _botClient.SendMessage(chatId, "Введите TelegramUsername (например: @Oleg)");
                    _commandState[chatId] = InputStage.AwaitingTelegramUsername;
                    break;
                case InputStage.AwaitingTelegramUsername:
                    if (!string.IsNullOrEmpty(messageText))
                    {
                        telegramUsername = messageText;
                        _commandState[chatId] = InputStage.ShowSuccessMessage;
                        await RestartCommand(chatId);
                    }
                    else
                    {
                        await _botClient.SendMessage(chatId, "Строка не должна быть пустой");
                        _commandState[chatId] = InputStage.RequestTelegramUsername;
                        await RestartCommand(chatId);
                    }
                    break;
                case InputStage.ShowSuccessMessage:
                    await _botClient.SendMessage(chatId, "Человек успешно добавлен!");
                    await _botClient.SendMessage(chatId, $"Итог:\nФИ: {fullName}\nДата рождения: {birthday}\nTelegram Username: {telegramUsername}");
                    fullName = string.Empty;
                    birthday = DateTime.MinValue;
                    telegramUsername = string.Empty;
                    _commandState.Remove(chatId);
                    break;
            }

        }

        private async Task HandleCallbackQuery(CallbackQuery callbackQuery, long chatId)
        {
            var data = callbackQuery.Data;

            if (data == CommandNames.CallbackSkipUsernameC)
            {
                _commandState[chatId] = InputStage.ShowSuccessMessage;
                await RestartCommand(chatId);
            }
            else
            {
                _commandState[chatId] = InputStage.RequestTelegramUsername;
                await RestartCommand(chatId);
            }

            return;
        }

        private async Task RestartCommand(long chatId)
        {
            await ExecuteAsync(new Update
            {
                Message = new Message
                {
                    Chat = new Chat
                    {
                        Id = chatId
                    }
                }
            });
        }

        private string CapitalizeWords(string input)
        {
            return string.Join(" ", input.Split(' ')
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        }

        private bool IsValidFullName(string fullName)
        {
            var array = fullName.Split(' ');
            if (array.Length == 2)
            {
                return true;
            }
            return false;
        }

        private bool IsValidDate(string inputDate, out DateTime birthdayDate)
        {
            return DateTime.TryParseExact(inputDate + ".2000", "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthdayDate);
        }

        private enum InputStage
        {
            Start,
            RequestFullName,
            AwaitingFullName,
            RequestBirthday,
            AwaitingBirthday,
            RequestUsernameChoice,
            RequestTelegramUsername,
            AwaitingTelegramUsername,
            ShowSuccessMessage
        }
    }
}
