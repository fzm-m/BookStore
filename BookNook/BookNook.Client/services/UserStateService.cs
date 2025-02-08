namespace BookNook.Client.services
{
    public class UserStateService
    {
        public bool IsLoggedIn { get; private set; }
        public string? UserName { get; private set; }
        public string? Token { get; private set; }
        public bool IsAdmin { get; private set; }

        public event Action OnChange;

        public void SetLoginState(bool isLoggedIn, string? username = null, bool isAdmin = false, string? token = null)
        {
            IsLoggedIn = isLoggedIn;
            UserName = username;
            IsAdmin = isAdmin;
            Token = token;
            NotifyStateChanged();
        }

        public void Login(string username, string token)
        {
            IsLoggedIn = true;
            UserName = username;
            Token = token;
            NotifyStateChanged();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            UserName = null;
            Token = null;
            IsAdmin = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
