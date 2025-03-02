namespace BirthdaysBot.BLL.Helpers
{
    public class Helper
    {
        public static async Task DeleteMessage(ITelegramBotClient botClient, long chatId, int messageId)
        {
            await botClient.DeleteMessage(chatId, messageId);
            await Task.Delay(100);
        }
    }
}
