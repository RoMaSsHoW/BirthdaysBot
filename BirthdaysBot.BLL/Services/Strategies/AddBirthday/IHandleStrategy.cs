namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    public interface IHandleStrategy
    {
        Task Handle(ITelegramBotClient botClient, Update update, long chatId, UserBirthdayInfo state);
    }
}
