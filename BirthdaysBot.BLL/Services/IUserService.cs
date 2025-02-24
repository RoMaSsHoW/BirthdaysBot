using BirthdaysBot.BLL.Models;
using Telegram.Bot.Types;

namespace BirthdaysBot.BLL.Services
{
    public interface IUserService
    {
        AppUser? GetUser(Update update);
    }
}
