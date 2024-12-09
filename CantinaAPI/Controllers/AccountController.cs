using CantinaAPI.Dtos;
using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CantinaAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var token = await _userService.LoginAsync(model);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
