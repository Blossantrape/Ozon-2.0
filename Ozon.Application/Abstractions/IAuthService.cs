using Ozon.Application.DTOs;
using Ozon.Core.Models;

namespace Ozon.Application.Abstractions
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto registerDto);
        Task<User> LoginAsync(LoginDto loginDto);
    }
}