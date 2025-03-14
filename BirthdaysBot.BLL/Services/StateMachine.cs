namespace BirthdaysBot.BLL.Services
{
    public class StateMachine
    {
        private readonly Dictionary<long, UserState> _userStates = new();

        public UserState GetUserState(long chatId)
        {
            return _userStates.ContainsKey(chatId) ? _userStates[chatId] : UserState.MainMenu;
        }

        public void SetUserState(long chatId, UserState state)
        {
            _userStates[chatId] = state;
        }

        public void ResetUserState(long chatId)
        {
            _userStates.Remove(chatId);
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
