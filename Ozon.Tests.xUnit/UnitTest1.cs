using Microsoft.AspNetCore.Mvc;
using Moq;
using Ozon.API.Controllers;
using Ozon.Application.Abstractions;
using Ozon.Core.Models;

namespace Ozon.Tests.xUnit;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductController(_mockService.Object);
    }

    [Fact]
    public void GetProductById_ReturnsOk_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Test Product", Price = 100 };

        _mockService.Setup(s => s.GetById(productId)).Returns(product);

        // Act
        var result = _controller.GetProductById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(productId, returnedProduct.Id);
    }
    
    [Fact]
    public void GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _mockService.Setup(s => s.GetById(productId)).Returns((Product)null);

        // Act
        var result = _controller.GetProductById(productId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // изменено на NotFoundObjectResult
        Assert.Equal(404, notFoundResult.StatusCode); // можно также проверить код статуса
    }

}