using System.Net.Http.Json;

namespace BookNook.Client.services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UserStateService _userState;

        public AuthService(HttpClient httpClient, UserStateService userState)
        {
            _httpClient = httpClient;
            _userState = userState;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/login", new
                {
                    UserName = username,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (loginResult?.Token != null)
                    {
                        // 设置Authorization header
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

                        // 获取用户详细信息
                        var userResponse = await _httpClient.GetAsync("api/Account/check-login");
                        if (userResponse.IsSuccessStatusCode)
                        {
                            var loginCheck = await userResponse.Content.ReadFromJsonAsync<LoginCheckResponse>();
                            if (loginCheck != null)
                            {
                                _userState.SetLoginState(true, loginCheck.Username, loginCheck.IsAdmin, loginResult.Token);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }

    public class LoginCheckResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
    }
}