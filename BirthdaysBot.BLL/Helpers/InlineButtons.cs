namespace BirthdaysBot.BLL.Helpers
{
    public class InlineButtons
    {
        private static InlineKeyboardMarkup yesOrNo = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Да", $"{CommandNames.CallbackYesC}"),
                InlineKeyboardButton.WithCallbackData("Нет", $"{CommandNames.CallbackNoC}")
            }
        });

        private static InlineKeyboardMarkup addOrSkipUsername = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Да", $"{CommandNames.CallbackAddUsernameC}"),
                InlineKeyboardButton.WithCallbackData("Нет", $"{CommandNames.CallbackSkipUsernameC}")
            }
        });

        public static InlineKeyboardMarkup YesOrNo => yesOrNo;

        public static InlineKeyboardMarkup AddOrSkipUsername => addOrSkipUsername;
    }
}