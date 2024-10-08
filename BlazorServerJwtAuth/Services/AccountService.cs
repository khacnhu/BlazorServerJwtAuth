using BlazorServerJwtAuth.Components.Pages;
using BlazorServerJwtAuth.DTOs;
using BlazorServerJwtAuth.Responses;
using BlazorServerJwtAuth.States;
using System.Reflection.Metadata;

namespace BlazorServerJwtAuth.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {

            GetProtectedClient();

            var response = await httpClient.GetAsync("api/Account/weather");
            bool check = CheckIfAuthorized(response);
            if (check)
            {
                await GetRefreshToken();
                return await GetWeatherForecasts();
            }

            return await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

        }

        private static bool CheckIfAuthorized (HttpResponseMessage httpResponseMessage)
        {
            if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return true;
            }
            return false;
        }

        private void GetProtectedClient()
        {
            if (Constants.JwtToken == "" || Constants.JwtToken == null) return;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", 
                Constants.JwtToken
            );

        }

        public void LogOut()
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
               "Bearer",
               Constants.JwtToken
           );
            

        }

        private async Task GetRefreshToken()
        {
            var response = await httpClient.PostAsJsonAsync("api/Account/refreshToken", new UserSession() { JwtToken = Constants.JwtToken });
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Constants.JwtToken = result!.JWTToken;
        
        }

        public async Task<LoginResponse> LoginService(LoginDTO loginDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/Account/login", loginDTO);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;
        }

        public async Task<RegistrationResponse> RegiserService(RegisterDTO registerDTO)
        {
          
            var response = await httpClient.PostAsJsonAsync("api/Account/register", registerDTO);
            var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
            return result!;
        }

        public async Task<LoginResponse> RefreshToken(UserSession userSession)
        {
            var response = await httpClient.PostAsJsonAsync("api/Account/refreshToken", userSession);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;

        }
    }
}
