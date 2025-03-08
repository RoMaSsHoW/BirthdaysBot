namespace BirthdaysBot.DAL.Entities;

public partial class Birthday
{
    public int BirthdayId { get; set; }

    public string? BirthdayName { get; set; }

    public DateOnly? BirthdayDate { get; set; }

    public string? BirthdayTelegramUsername { get; set; }

    public long? UserChatId { get; set; }
}
