using CantinaAPI.Dtos;

namespace CantinaAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterRequestDto model);
        Task<string> LoginAsync(LoginRequestDto model);
    }
}

