using BlazorServerJwtAuth.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorServerJwtAuth.States
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonynous = new(new ClaimsPrincipal());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Constants.JwtToken))
                {
                    return await Task.FromResult(new AuthenticationState(anonynous));
                }

                var getUserClaims = DecryptToken(Constants.JwtToken);
                if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonynous));

                var claimsPrincipal = SetClaimPricipal(getUserClaims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            } catch
            {
                return await Task.FromResult(new AuthenticationState(anonynous));

            }

        }


        public static ClaimsPrincipal SetClaimPricipal(CustomerUserClaims claims)
        {
            if (claims.Email is null)
            {
                return new ClaimsPrincipal();
            }

            return new ClaimsPrincipal(new ClaimsIdentity(
                    new List<Claim>
                    {
                        new(ClaimTypes.Name, claims.Name!),
                        new(ClaimTypes.Email, claims.Email!)
                    }, "JwtAuth"

                ));

        }


        public async void UpdateAuthenticationState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                Constants.JwtToken = jwtToken;
                var getUserClaims = DecryptToken(jwtToken);
                claimsPrincipal = SetClaimPricipal(getUserClaims);
            }
            else
            {
                Constants.JwtToken = null!;

            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

        }

        public static CustomerUserClaims DecryptToken(string jwtToken) 
        {
            if (string.IsNullOrEmpty(jwtToken)) return new CustomerUserClaims();
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwtToken);


            var name = token.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);

            return new CustomerUserClaims(name!.Value, email!.Value);


        }



    }
}
