namespace BirthdaysBot.BLL.Models
{
    public class UserBirthdayInfo
    {
        public string? FullName { get; set; }

        public DateTime Birthday { get; set; }

        public string? TelegramUsername { get; set; }

        public bool IsComplete => 
            !string.IsNullOrEmpty(FullName) &&
            Birthday != DateTime.MinValue &&
            !string.IsNullOrEmpty(TelegramUsername);
    }
}
