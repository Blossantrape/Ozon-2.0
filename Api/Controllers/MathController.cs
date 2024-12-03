using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MathController : ControllerBase
{
    private readonly ICalculatorService _calculatorService;

    public MathController(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    /// <summary>
    /// Выполняет сложение двух чисел.
    /// </summary>
    /// <param name="a">Первое число.</param>
    /// <param name="b">Второе число.</param>
    /// <returns>Результат сложения чисел.</returns>
    /// <response code="200">Возвращает результат операции.</response>
    /// <response code="400">Если входные данные некорректны.</response>
    [HttpGet("add")]
    public IActionResult Add(double a, double b)
    {
        var result = _calculatorService.Add(a, b);
        return Ok(new { Result = result });
    }

    [HttpGet("subtract")]
    public IActionResult Subtract(double a, double b)
    {
        var result = _calculatorService.Subtract(a, b);
        return Ok(new { Result = result });
    }

    [HttpGet("multiply")]
    public IActionResult Multiply(double a, double b)
    {
        var result = _calculatorService.Multiply(a, b);
        return Ok(new { Result = result });
    }

    [HttpGet("divide")]
    public IActionResult Divide(double a, double b)
    {
        try
        {
            var result = _calculatorService.Divide(a, b);
            return Ok(new { Result = result });
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}