using System.IdentityModel.Tokens.Jwt;

namespace Ozon.API.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;
                context.Items["User"] = userId;
            }
            catch
            {
                // Игнорируем, так как токен может отсутствовать.
            }
        }

        await _next(context);
    }
}