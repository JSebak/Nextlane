using Business.Models;
using Business.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;

namespace ProductManagement.Tests.ProductAPI
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WithProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 10.0 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 20.0 }
            };

            _mockProductService.Setup(service => service.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(products);

        }

        [Fact]
        public async Task GetProductById_ShouldReturnOkResult_WithProduct()
        {
            // Arrange
            var product = new ProductDto { Id = 1, Name = "Product 1", Price = 10.0 };

            _mockProductService.Setup(service => service.GetById(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetById(It.IsAny<int>()))
                .Throws(new ArgumentOutOfRangeException());

            // Act
            var result = await _controller.GetProductById(0);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var newProduct = new ProductDetailsModel { Name = "New Product", Price = 30.0 };

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be("Product Added successfully");
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidProduct = new ProductDetailsModel { Name = "", Price = -10.0 };

            _mockProductService.Setup(service => service.Add(invalidProduct))
                .Throws(new ArgumentException("Invalid product details"));

            // Act
            var result = await _controller.CreateProduct(invalidProduct);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid product details");
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var updatedProduct = new ProductDetailsModel { Name = "Updated Product", Price = 40.0 };

            // Act
            var result = await _controller.UpdateProduct(1, updatedProduct);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be("Product updated successfully");
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var updatedProduct = new ProductDetailsModel { Name = "Updated Product", Price = 40.0 };

            _mockProductService.Setup(service => service.Update(It.IsAny<int>(), updatedProduct))
                .Throws(new InvalidOperationException("Product not found"));

            // Act
            var result = await _controller.UpdateProduct(1, updatedProduct);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFoundResult.Value.Should().Be("Product not found");
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            _mockProductService.Setup(service => service.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be("Product deleted successfully");
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.Delete(It.IsAny<int>()))
                .Throws(new InvalidOperationException("Product not found"));

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFoundResult.Value.Should().Be("Product not found");
        }
    }
}

