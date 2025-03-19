
namespace BirthdaysBot.BLL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> UserExistsAsync(long chatId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(v => v.UserChatId == chatId);

            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
