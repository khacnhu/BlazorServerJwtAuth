using BCrypt.Net;
using BlazorServerJwtAuth.Data;
using BlazorServerJwtAuth.DTOs;
using BlazorServerJwtAuth.Models;
using BlazorServerJwtAuth.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace BlazorServerJwtAuth.Repos
{
    public class AccountRepo : IAccountRepo
    {
        private readonly AppDbContext context;
        private readonly IConfiguration config;

        public AccountRepo(AppDbContext context, IConfiguration config)
        {   
            this.context = context;
            this.config = config;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO loginDTO)
        {
            var findUser = await GetUser(loginDTO.Email);

            if (findUser == null) return new LoginResponse(false, "User not found or not register");
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, findUser.Password)) return new LoginResponse(false, "wrong password");

            string jwtToken = GenerateToken(findUser);
            return new LoginResponse(true, "Login Successfully", jwtToken);

        }

        public async Task<RegistrationResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            var findUser = await GetUser(registerDTO.Email);
            if (findUser != null) return new RegistrationResponse(false, "User already existed");

            await context.Users.AddAsync(
               new ApplicationUser()
               {
                   Name = registerDTO.Name,
                   Email = registerDTO.Email,
                   Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
               }
               );
            await context.SaveChangesAsync();

            return new RegistrationResponse(true, "Register Successfully");
        }

        private async Task<ApplicationUser?> GetUser(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Email == email);
            return user != null ? user : null;
        }

        private string GenerateToken(ApplicationUser user)
        {
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
          {
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),

            };

            var token = new JwtSecurityToken(
                   issuer: config["Jwt:Issuer"],
                   audience: config["Jwt:Audience"],
                   claims: userClaims,
                   expires: DateTime.Now.AddDays(2),
                   signingCredentials: credentials
               );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
