using BlazorServerJwtAuth.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorServerJwtAuth.Services
{
    public  static class DecryptJWTService
    {
        public static CustomerUserClaims DecryptToken(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken)) return new CustomerUserClaims();
                var handler = new JwtSecurityTokenHandler();

                var token = handler.ReadJwtToken(jwtToken);


                var name = token.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name);
                var email = token.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);
                var role = token.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role);

                return new CustomerUserClaims(name!.Value, email!.Value, role!.Value);


            } catch
            {
                return null!;
            }


        }
    }
}
