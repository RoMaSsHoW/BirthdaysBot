namespace BirthdaysBot.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(long chatId);
    }
}
