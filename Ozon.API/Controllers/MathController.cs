using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozon.API.Services;

namespace Ozon.API.Controllers
{
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

        [HttpGet("add")]
        public async Task<IActionResult> AddAsync(double a, double b)
        {
            var result = await _calculatorService.AddAsync(a, b);
            return Ok(new { Result = result });
        }

        [HttpGet("subtract")]
        public async Task<IActionResult> SubtractAsync(double a, double b)
        {
            var result = await _calculatorService.SubtractAsync(a, b);
            return Ok(new { Result = result });
        }

        [HttpGet("multiply")]
        public async Task<IActionResult> MultiplyAsync(double a, double b)
        {
            var result = await _calculatorService.MultiplyAsync(a, b);
            return Ok(new { Result = result });
        }

        [HttpGet("divide")]
        public async Task<IActionResult> DivideAsync(double a, double b)
        {
            try
            {
                var result = await _calculatorService.DivideAsync(a, b);
                return Ok(new { Result = result });
            }
            catch (DivideByZeroException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}