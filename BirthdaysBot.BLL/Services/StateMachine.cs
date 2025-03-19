namespace BirthdaysBot.BLL.Services
{
    public class StateMachine
    {
        private static readonly Dictionary<long, UserState> _userStates = new();

        public static UserState GetUserState(long chatId)
        {
            return _userStates.ContainsKey(chatId) ? _userStates[chatId] : UserState.MainMenu;
        }

        public static void SetUserState(long chatId, UserState state)
        {
            _userStates[chatId] = state;
        }

        public static void ResetUserState(long chatId)
        {
            _userStates[chatId] = UserState.MainMenu;
        }
    }

    public enum UserState
    {
        MainMenu,
        GettingBirthday,
        AddingBirthday,
        UpdatingBirthday,
        DeletingBirthday
    }
}
