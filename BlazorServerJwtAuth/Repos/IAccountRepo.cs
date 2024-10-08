using BlazorServerJwtAuth.DTOs;
using BlazorServerJwtAuth.Responses;

namespace BlazorServerJwtAuth.Repos
{
    public interface IAccountRepo
    {
        Task<RegistrationResponse> RegisterAsync(RegisterDTO registerDTO);
        Task<LoginResponse> LoginAsync(LoginDTO loginDTO);
        LoginResponse RefreshToken(UserSession userSession);

    }
}
