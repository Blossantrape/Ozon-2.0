using Ozon.Application.DTOs;
using Ozon.Core.Models;
using Ozon.DataAccess.Context;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Ozon.Application.Abstractions;

namespace Ozon.Application.Services
{
    public class JwtTokenService : IAuthService
    {
        private readonly AppDbContext _context;

        public JwtTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            // Проверка на существование пользователя
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
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
            await _context.SaveChangesAsync(); // Асинхронное сохранение в базу данных
            return user;
        }

        public async Task<User> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}