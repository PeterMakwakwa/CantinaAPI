using CantinaAPI.Dtos;
using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CantinaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var token = await _userService.RegisterAsync(model);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            return BadRequest("Registration failed.");
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

        [HttpGet("login-google")]
        [AllowAnonymous]
        public IActionResult LoginGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var token = await _userService.HandleGoogleResponseAsync(HttpContext);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            return BadRequest("Google authentication failed.");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
