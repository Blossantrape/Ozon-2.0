using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozon.Application.Abstractions;
using Ozon.Application.DTOs;
using Ozon.Core.Models;

namespace Ozon.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        /*[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        [HttpGet("GetProductById/{uuid}")]
        /*[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<IActionResult> GetProductById(Guid uuid)
        {
            try
            {
                var product = await _productService.GetByIdAsync(uuid);
                if (product == null)
                    return NotFound( /*$"Продукт с ID {uuid} не найден."*/);

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        [HttpPost("AddProduct")]
        /*[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = new Product
                {
                    Id = Guid.NewGuid(), // Генерируем ID на сервере
                    Name = addProductDto.Name,
                    Description = addProductDto.Description,
                    Price = addProductDto.Price,
                    StockQuantity = addProductDto.StockQuantity
                };

                await _productService.AddAsync(product);

                return CreatedAtAction(
                    nameof(GetProductById),
                    new { uuid = product.Id }, // "uuid" вместо "id"
                    product
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        [HttpPut("UpdateProduct/{uuid}")]
        /*[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<IActionResult> UpdateProduct(Guid uuid, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (uuid != product.Id)
                    return BadRequest("ID в URL и модели не совпадают.");

                var existingProduct = await _productService.GetByIdAsync(uuid);
                if (existingProduct == null)
                    return NotFound($"Продукт с ID {uuid} не найден.");

                await _productService.UpdateAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        [HttpDelete("DeleteProduct/{uuid}")]
        /*[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/
        public async Task<IActionResult> DeleteProduct(Guid uuid)
        {
            try
            {
                var existingProduct = await _productService.GetByIdAsync(uuid);
                if (existingProduct == null)
                    return NotFound($"Продукт с ID {uuid} не найден.");

                await _productService.DeleteAsync(uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}