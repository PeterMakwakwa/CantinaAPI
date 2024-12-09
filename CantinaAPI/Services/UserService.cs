using CantinaAPI.Dtos;
using CantinaAPI.Models;
using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CantinaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> RegisterAsync(RegisterRequestDto model)
        {
            try
            {
                var user = new UserModel { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    _logger.LogError($"User registration failed: {result.Errors}");
                    return string.Empty;
                }

                return "Registration successful";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An issue has occured during user registration");
                return string.Empty;
            }
        }

        public async Task<string> LoginAsync(LoginRequestDto model)
        {
            var token = string.Empty;
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Invalid login attempt with email: {model.Email}");
                    return token;
                }                   

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Invalid login attempt with email: {model.Email}, user: {user.FullName}");
                }

                 token = GenerateJwtToken(user);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An issue has occured during user registration");
                return token;
            }
           
        }

        private string GenerateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }       
    }

}
