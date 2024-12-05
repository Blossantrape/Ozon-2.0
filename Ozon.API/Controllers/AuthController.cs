using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ozon.Application.DTOs;
using Ozon.Core.Models;
using Ozon.DataAccess.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ozon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            if (await _dbContext.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return Conflict("Username already exists.");
            }

            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                PasswordHash = hashedPassword,
                Role = registerDto.Role ?? "User"
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }


        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
