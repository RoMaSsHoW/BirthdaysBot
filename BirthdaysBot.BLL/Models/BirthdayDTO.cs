namespace BirthdaysBot.BLL.Models
{
    public class BirthdayDTO
    {
        public int BirthdayId { get; set; }

        public long? UserChatId { get; set; }

        public string? BirthdayName { get; set; }

        public DateTime? BirthdayDate { get; set; }

        public string? BirthdayTelegramUsername { get; set; }
    }
}
