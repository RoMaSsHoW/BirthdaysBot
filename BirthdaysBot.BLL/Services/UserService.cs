namespace BirthdaysBot.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly AppUser admin = new AppUser
        {
            ChatId = 525904829,
            Username = "Сахаров Роман",
            TelegramUsername = "@Roman_S5"
        };
        private readonly AppUser subAdmin = new AppUser
        {
            ChatId = 1955716984,
            Username = "Нияз"
        };

        public AppUser? GetUser(Update update)
        {
            if (update == null)
            {
                return null;
            }

            AppUser newUser = null;

            if (update.Type == UpdateType.CallbackQuery)
            {
                newUser = new AppUser
                {
                    ChatId = update.CallbackQuery!.Message!.Chat.Id,
                    Username = $"{update.CallbackQuery.Message.From!.FirstName} {update.CallbackQuery.Message.From.LastName ?? ""}".Trim(),
                    TelegramUsername = update.CallbackQuery.From.Username ?? ""
                };
            }
            else if (update.Type == UpdateType.Message)
            {
                newUser = new AppUser
                {
                    ChatId = update.Message!.Chat.Id,
                    Username = $"{update.Message.Chat.FirstName} {update.Message.Chat.LastName ?? ""}".Trim(),
                    TelegramUsername = update.Message.Chat.Username ?? ""
                };
            }

            if (newUser != null && newUser.ChatId == admin.ChatId)
            {
                return admin;
            }
            else if (newUser != null && newUser.ChatId == subAdmin.ChatId)
            {
                return subAdmin;
            }
            return null;
        }
    }
}
