namespace BirthdaysBot.BLL.Helpers
{
    public class ReplyButtons
    {
        private static ReplyKeyboardMarkup mainKeybord = new(new[]
        {
            //new KeyboardButton[] { CommandNames.MyGroupsRuC },
            new KeyboardButton[] { CommandNames.AddBirthdayRuC, CommandNames.ShowBirthdaysRuC},
            new KeyboardButton[] { CommandNames.DeleteBirthdayRuC, CommandNames.UpdateBirthdayRuC }
        })
        {
            ResizeKeyboard = true // Клавиатура будет адаптирована под экран
        };

        public static ReplyKeyboardMarkup MainKeybord => mainKeybord;
    }
}
