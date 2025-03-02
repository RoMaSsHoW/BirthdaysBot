namespace BirthdaysBot.BLL.Services
{
    public interface IUpdateHandler
    {
        Task Execute(Update update);
    }
}
