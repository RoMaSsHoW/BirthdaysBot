namespace BirthdaysBot.BLL.Services.Strategies.DeleteBirthday
{
    public interface IDeleteStrategy
    {
        Task ExecuteAsync(ITelegramBotClient botClient, Update update, IBirthdayRepository birthdayRepository, long chatId);
    }
}
