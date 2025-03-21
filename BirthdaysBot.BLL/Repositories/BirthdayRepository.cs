﻿namespace BirthdaysBot.BLL.Repositories
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

        public async Task<IEnumerable<BirthdayDTO>> GetBirthdaysAsync(long chatId)
        {
            var birthdays = await _dbContext.Birthdays
                .Where(v => v.UserChatId == chatId)
                .OrderBy(v => v.BirthdayDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BirthdayDTO>>(birthdays);
        }

        public async Task<IEnumerable<BirthdayDTO>> GetAllBirthdaysAsync()
        {
            var birthdays = await _dbContext.Birthdays
                .OrderBy(b => b.BirthdayDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BirthdayDTO>>(birthdays);
        }

        public async Task<BirthdayDTO> GetBirthdayAsync(int birthdayId, long chatId)
        {
            var birthday = await _dbContext.Birthdays
                .FirstOrDefaultAsync(v => v.BirthdayId == birthdayId && v.UserChatId == chatId);

            return _mapper.Map<BirthdayDTO>(birthday);
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

            var birthdays = await _dbContext.Birthdays
                .Where(v => v.BirthdayName == birthdayInfo.FullName)
                .Where(v => v.BirthdayDate == birthdayInfo.Birthday)
                .Where(v => v.UserChatId == chatId)
                .ToListAsync();

            if (birthdays.Count > 0) return true;

            return false;
        }

        public async Task<bool> UpdateBirthdayAsync(BirthdayDTO birthdayDTO, long chatId)
        {
            var existingBirthday = await _dbContext.Birthdays
                .FirstOrDefaultAsync(v => v.BirthdayId == birthdayDTO.BirthdayId && v.UserChatId == chatId);

            if (existingBirthday == null) return false;

            _mapper.Map(birthdayDTO, existingBirthday);

            _dbContext.Birthdays.Update(existingBirthday);

            await _dbContext.SaveChangesAsync();

            return true;
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
