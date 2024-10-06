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

            if (Constants.JwtToken == null || Constants.JwtToken == "")
            {
                return null!;
            }

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.JwtToken);

            var response = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/Account/weather");
            return response!;
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
    }
}
