namespace BirthdaysBot.BLL.Helpers
{
    public class TestData
    {
        private IEnumerable<BirthdayDTO> testBirthdays = new List<BirthdayDTO>
        {
            new BirthdayDTO
            {
                BirthdayId = 1,
                UserChatId = 525904829,
                BirthdayName = "Test1",
                BirthdayDate = new DateTime(2004, 02, 14),
                BirthdayTelegramUsername = "-",
            },
            new BirthdayDTO
            {
                BirthdayId = 2,
                UserChatId = 525904829,
                BirthdayName = "Test2",
                BirthdayDate = new DateTime(2004, 06, 06),
                BirthdayTelegramUsername = "-",
            },
            new BirthdayDTO
            {
                BirthdayId = 3,
                UserChatId = 525904829,
                BirthdayName = "Test3",
                BirthdayDate = new DateTime(2004, 03, 08),
                BirthdayTelegramUsername = "-",
            },
            new BirthdayDTO
            {
                BirthdayId = 4,
                UserChatId = 525904829,
                BirthdayName = "Test3",
                BirthdayDate = DateTime.Now.AddDays(3),
                BirthdayTelegramUsername = "-",
            },
        };

        public IEnumerable<BirthdayDTO> TestBirthdays => testBirthdays;
    }
}
