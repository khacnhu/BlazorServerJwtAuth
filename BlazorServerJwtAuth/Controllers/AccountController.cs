using BlazorServerJwtAuth.DTOs;
using BlazorServerJwtAuth.Repos;
using BlazorServerJwtAuth.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo accountRepo;
        public AccountController(IAccountRepo accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegisterDTO registerDTO)
        {
            var result = await accountRepo.RegisterAsync(registerDTO);
            return Ok(result);
        }

        [HttpPost("login")]        
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginDTO loginDTO)
        {
            var result = await accountRepo.LoginAsync(loginDTO);
            return result;
        }

        [HttpGet("auth")]
        [Authorize]
        public string Hello()
        {
            return "Auth SuccessFully Test";
        }

        [HttpGet("weather")]
        [Authorize]
        public ActionResult<WeatherForecast[]> GetWeatherForeCast()
        {

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToArray());
        }
        

        
        
        

        

    }
}
