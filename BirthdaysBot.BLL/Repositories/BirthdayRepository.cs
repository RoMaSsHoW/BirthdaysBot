namespace BirthdaysBot.BLL.Repositories
{
    public class BirthdayRepository : IBirthdayRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BirthdayRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateBirthdayAsync(UserBirthdayInfo birthdayInfo, long chatId)
        {
            var newBirthday = new Birthday
            {
                UserChatId = chatId,
                BirthdayName = birthdayInfo.FullName,
                BirthdayDate = birthdayInfo.Birthday,
                BirthdayTelegramUsername = birthdayInfo.TelegramUsername,
            };

            _dbContext.Birthdays.Add(newBirthday);

            await _dbContext.SaveChangesAsync();

            var query = _dbContext.Birthdays.AsQueryable();

            query = query.Where(v => v.BirthdayName == birthdayInfo.FullName);

            query = query.Where(v => v.BirthdayDate == birthdayInfo.Birthday);

            var birthdays = await query.ToListAsync();

            if (birthdays.Count > 0) return true;

            return false;
        }
    }
}
