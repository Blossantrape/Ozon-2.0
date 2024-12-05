using Ozon.Application.DTOs;
using Ozon.Core.Models;

namespace Ozon.Application.Abstractions
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto registerDto);
        Task<User> Login(LoginDto loginDto);
    }
}