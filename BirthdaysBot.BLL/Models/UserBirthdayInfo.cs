namespace BirthdaysBot.BLL.Models
{
    public class UserBirthdayInfo
    {
        public string? FullName { get; set; }

        public DateOnly Birthday { get; set; }

        public string? TelegramUsername { get; set; }

        public bool IsComplete => 
            !string.IsNullOrEmpty(FullName) &&
            Birthday != DateOnly.MinValue &&
            !string.IsNullOrEmpty(TelegramUsername);
    }
}
