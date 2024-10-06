using BlazorServerJwtAuth.DTOs;
using BlazorServerJwtAuth.Responses;

namespace BlazorServerJwtAuth.Services
{
    public interface IAccountService
    {
        Task<RegistrationResponse> RegiserService(RegisterDTO registerDTO);
        Task<LoginResponse> LoginService(LoginDTO loginDTO);

        Task<WeatherForecast[]> GetWeatherForecasts();

    }
}
