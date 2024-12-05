using Microsoft.EntityFrameworkCore;
using Ozon.API;
using Ozon.API.Services;
using Ozon.Application.Abstractions;
using Ozon.Application.Services;
using Ozon.Application.Settings;
using Ozon.Core.Abstractions;
using Ozon.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, JwtTokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();