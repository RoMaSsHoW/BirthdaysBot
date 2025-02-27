using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services.Strategies.AddBirthday
{
    public interface IHandleStrategy
    {
        Task Handle(Update update, long chatId);
    }
}
