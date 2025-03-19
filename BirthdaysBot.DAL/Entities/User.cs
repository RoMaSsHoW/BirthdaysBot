namespace BirthdaysBot.DAL.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public long? UserChatId { get; set; }
}
