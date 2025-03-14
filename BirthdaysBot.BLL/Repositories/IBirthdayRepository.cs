namespace BirthdaysBot.BLL.Repositories
{
    public interface IBirthdayRepository
    {
        Task<IEnumerable<BirthdayDTO>> GetBirthdaysAsync(long chatId);

        Task<BirthdayDTO> GetBirthdayAsync(int birthdayId, long chatId);

        Task<bool> CreateBirthdayAsync(UserBirthdayInfo birthdayInfo, long chatId);

        Task<bool> UpdateBirthdayAsync(BirthdayDTO birthdayDTO, long chatId);

        Task<bool> DeleteBirthdayAsync(int birthdayId, long chatId);
    }
}
