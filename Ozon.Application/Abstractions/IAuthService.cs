using Ozon.Application.DTOs;
using Ozon.Core.Models;

namespace Ozon.Application.Abstractions
{
    public interface IAuthService
    {
        User Register(RegisterDto registerDto);
        User Login(LoginDto loginDto);
    }
}