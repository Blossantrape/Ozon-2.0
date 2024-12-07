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

        /// <summary>
        /// Получить список всех продуктов.
        /// </summary>
        /// <returns>Список продуктов.</returns>
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить продукт по ID.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <returns>Продукт или ошибка 404.</returns>
        [HttpGet("GetProductById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductById(Guid id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                    return NotFound(/*$"Продукт с ID {id} не найден."*/);

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавить новый продукт.
        /// </summary>
        /// <param name="product">Модель продукта.</param>
        /// <param name="addProductDto"></param>
        /// <returns>Созданный продукт с его ID.</returns>
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddProduct([FromBody] AddProductDto addProductDto)
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

                _productService.Add(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновить данные продукта.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <param name="product">Модель продукта с обновленными данными.</param>
        /// <returns>Статус выполнения операции.</returns>
        [HttpPut("UpdateProduct/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(Guid id, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != product.Id)
                    return BadRequest("ID в URL и модели не совпадают.");

                var existingProduct = _productService.GetById(id);
                if (existingProduct == null)
                    return NotFound($"Продукт с ID {id} не найден.");

                _productService.Update(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Удалить продукт по ID.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <returns>Статус выполнения операции.</returns>
        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                var existingProduct = _productService.GetById(id);
                if (existingProduct == null)
                    return NotFound($"Продукт с ID {id} не найден.");

                _productService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка: {ex.Message}");
            }
        }
    }
}
