using Business.Models;
using Business.Services;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ProductManagement.Tests.Business
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_A_List_Of_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 10.0 },
                new Product { Id = 2, Name = "Product B", Price = 20.0 }
            };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Product A" && p.Price == 10.0);
            result.Should().Contain(p => p.Name == "Product B" && p.Price == 20.0);
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Return_An_Empty_List_Of_Products()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetAll();

            // Assert
            result.Should().BeEmpty();
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Throw_An_ExceptionAsync()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _productService.GetAll();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }



        [Fact]
        public async Task GetById_ValidId_ReturnsProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.99 };
            _mockRepository.Setup(r => r.GetById(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetById(productId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(10.99);

            _mockRepository.Verify(r => r.GetById(productId), Times.Once);
        }

        [Fact]
        public async Task GetById_ProductNotFound_ReturnsNull()
        {
            // Arrange
            var productId = 999;
            _mockRepository.Setup(r => r.GetById(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetById(productId);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetById(productId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_InvalidId_ThrowsArgumentOutOfRangeExceptionAsync(int invalidId)
        {
            // Act
            Func<Task> act = async () => await _productService.GetById(invalidId);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>().WithParameterName("Invalid value for id.");
            _mockRepository.Verify(r => r.GetById(invalidId), Times.Never);
        }



        [Fact]
        public async Task Add_ShouldAddProduct_WhenValid()
        {
            // Arrange
            var newProduct = new ProductDetailsModel { Name = "Product A", Price = 10.0 };
            Product addedProduct = null!;

            _mockRepository
                .Setup(repo => repo.Add(It.IsAny<Product>()))
                .Callback<Product>(p => addedProduct = p);

            // Act
            await _productService.Add(newProduct);

            // Assert
            addedProduct.Should().NotBeNull();
            addedProduct.Name.Should().Be("Product A");
            addedProduct.Price.Should().Be(10.0);
            _mockRepository.Verify(repo => repo.Add(It.IsAny<Product>()), Times.Once);
        }

        [Theory]
        [InlineData("", 5.3)]
        [InlineData("", -5.3)]
        [InlineData(null, 5.3)]
        [InlineData("Nombre", -8.0)]
        [InlineData("Nombre", null)]
        [InlineData(null, null)]
        public async Task Add_ShouldThrowArgumentException_WhenValidationFails(string? name, double? price)
        {
            // Arrange
            var invalidProduct = new ProductDetailsModel { Name = name, Price = price };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.Add(invalidProduct));

            // Assert
            if (string.IsNullOrEmpty(name))
            {
                Assert.Contains("Invalid product name", exception.Message);
            }
            if (price == null || price < 0)
            {
                Assert.Contains("Invalid product price", exception.Message);
            }

            _mockRepository.Verify(repo => repo.Add(It.Is<Product>(p => p.Name == name && p.Price == price)), Times.Never);
        }

        [Fact]
        public async Task Add_ShouldRethrowException_WhenRepositoryFails()
        {
            // Arrange
            var newProduct = new ProductDetailsModel { Name = "Product A", Price = 10.0 };
            _mockRepository.Setup(repo => repo.Add(It.IsAny<Product>())).ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _productService.Add(newProduct);

            // Assert
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithMessage("Database error");

            _mockRepository.Verify(repo => repo.Add(It.Is<Product>(p => p.Name == newProduct.Name && p.Price == newProduct.Price)), Times.Once);
        }



        [Fact]
        public async Task Update_ShouldUpdateProduct_WhenValid()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Product1", Price = 10.0 };
            var updatedProduct = new ProductDetailsModel { Name = "Updated Product", Price = 15.0 };

            _mockRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync(existingProduct);

            // Act
            await _productService.Update(productId, updatedProduct);

            // Assert
            _mockRepository.Verify(repo => repo.GetById(productId), Times.Once);

            existingProduct.Name.Should().Be("Updated Product");
            existingProduct.Price.Should().Be(15.0);

            _mockRepository.Verify(repo => repo.Update(It.Is<Product>(p => p.Id == productId && p.Name == updatedProduct.Name && p.Price == updatedProduct.Price)), Times.Once);
        }

        [Theory]
        [InlineData("", -5.0)]
        [InlineData("", 5.0)]
        [InlineData("Updated", -5.0)]
        [InlineData(null, -5.0)]
        public async Task Update_ShouldThrowArgumentException_WhenValidationFails(string? name, double? price)
        {
            // Arrange
            var invalidProduct = new ProductDetailsModel { Name = name, Price = price };
            var existingProduct = new Product { Id = 1, Price = 1.2, Name = "TestProd" };

            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingProduct);

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.Update(1, invalidProduct));

            // Assert
            if (name != null && string.IsNullOrEmpty(name))
            {
                Assert.Contains("Invalid product name", exception.Message);
            }
            if (price == null || price < 0)
            {
                Assert.Contains("Invalid product price", exception.Message);
            }

            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowInvalidOperationException_WhenProductNotFound()
        {
            // Arrange
            var productId = 999;
            var updatedProduct = new ProductDetailsModel { Name = "Product A", Price = 10.0 };

            _mockRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _productService.Update(productId, updatedProduct);

            // Assert
            var exception = await act.Should().ThrowAsync<InvalidOperationException>();
            exception.WithMessage("There's no product associated with the id.");

            _mockRepository.Verify(repo => repo.Update(It.Is<Product>(p => p.Id == 1 && p.Name == updatedProduct.Name && p.Price == updatedProduct.Price)), Times.Never);
        }



        [Fact]
        public async Task Delete_ShouldDeleteProduct_WhenIdIsValid()
        {
            // Arrange
            var productId = 1;
            _mockRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync(new Product { Id = productId });

            // Act
            await _productService.Delete(productId);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(productId), Times.Once);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(0)]
        public async Task Delete_ShouldThrowArgumentOutOfRangeException_WhenIdIsInvalid(int id)
        {
            // Act
            Func<Task> act = async () => await _productService.Delete(id);

            // Assert
            var exception = await act.Should().ThrowAsync<ArgumentException>();
            exception.WithMessage("Invalid id value.");
            _mockRepository.Verify(repo => repo.Delete(id), Times.Never);
        }

        [Fact]
        public async Task Delete_ShouldThrowInvalidOperationException_WhenProductNotFound()
        {
            // Arrange
            var productId = 999;
            _mockRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _productService.Delete(productId);

            // Assert
            var exception = await act.Should().ThrowAsync<InvalidOperationException>();
            exception.WithMessage($"No product found with id {productId}");
            _mockRepository.Verify(repo => repo.Delete(productId), Times.Never);
        }
    }
}
