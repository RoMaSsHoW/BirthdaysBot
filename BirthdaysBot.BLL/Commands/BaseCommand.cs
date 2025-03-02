namespace BirthdaysBot.BLL.Commands
{
    public abstract class BaseCommand
    {
        public abstract string CommandName { get; }
        public abstract Task ExecuteAsync(Update update);
    }
}