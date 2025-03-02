namespace BirthdaysBot.BLL.Services
{
    public interface IUserService
    {
        AppUser? GetUser(Update update);
    }
}
