namespace BirthdaysBot.BLL.Models
{
    public class BirthdayDTO
    {
        public int BirthdayId { get; set; }

        public long? UserChatId { get; set; }

        public string? BirthdayName { get; set; }

        public DateOnly? BirthdayDate { get; set; }

        public string? BirthdayTelegramUsername { get; set; }
    }
}
