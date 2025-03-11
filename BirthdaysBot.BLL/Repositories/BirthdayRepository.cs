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

            query = query.Where(v => v.UserChatId == chatId);

            var birthdays = await query.ToListAsync();

            if (birthdays.Count > 0) return true;

            return false;
        }

        public async Task<IEnumerable<BirthdayDTO>> GetBirthdaysAsync(long chatId)
        {
            var query = _dbContext.Birthdays.AsQueryable();

            query = query.Where(v => v.UserChatId == chatId);

            var birthdays = await query.ToListAsync();

            return _mapper.Map<IEnumerable<BirthdayDTO>>(birthdays);
        }

        public async Task<bool> DeleteBirthdayAsync(int birthdayId, long chatId)
        {
            var birthday = await _dbContext.Birthdays
                .Where(v => v.BirthdayId == birthdayId)
                .Where(v => v.UserChatId == chatId)
                .ToListAsync();

            if (birthday.Any())
            {
                _dbContext.Birthdays.RemoveRange(birthday);

                await _dbContext.SaveChangesAsync();
            }

            var birthdays = await _dbContext.Birthdays
                .Where(v => v.BirthdayId == birthdayId)
                .Where(v => v.UserChatId == chatId)
                .ToListAsync();

            if (birthdays.Count == 0) return true;

            return false;
        }
    }
}
