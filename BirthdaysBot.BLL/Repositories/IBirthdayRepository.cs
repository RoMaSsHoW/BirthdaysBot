namespace BirthdaysBot.BLL.Repositories
{
    public interface IBirthdayRepository
    {
        Task<bool> CreateBirthdayAsync(UserBirthdayInfo birthdayInfo, long chatId);

        Task<IEnumerable<BirthdayDTO>> GetBirthdaysAsync(long chatId); 

        Task<bool> DeleteBirthdayAsync(int birthdayId, long chatId);
    }
}
