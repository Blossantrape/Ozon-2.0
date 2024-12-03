using Ozon.Application.Abstractions;
using Ozon.Application.DTOs;
using Ozon.Core.Models;
using Ozon.DataAccess.Context;

namespace Ozon.Application.Services;

public class JwtTokenService : IAuthService
{
    private readonly AppDbContext _context;

    public JwtTokenService(AppDbContext context)
    {
        _context = context;
    }

    public User Register(RegisterDto registerDto)
    {
        if (_context.Users.Any(u => u.Username == registerDto.Username))
        {
            return null; // Пользователь уже существует
        }

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role // Устанавливаем роль
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }


    public User Login(LoginDto loginDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}