using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly ProductRepository _repository;
        private readonly ProductManagementDbContext _context;

        public ProductRepositoryTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ProductManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductManagementTestDatabase")
                .Options;

            _context = new ProductManagementDbContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 10.0 },
                new Product { Id = 2, Name = "Product B", Price = 20.0 }
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAll();

            // Assert
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmpty()
        {
            // Arrange
            var products = new List<Product>
            {
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAll();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var product = new Product { Id = 3, Name = "Product A", Price = 10.0 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetById(3);

            // Assert
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotExists()
        {

            // Act
            var result = await _repository.GetById(99);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Add_ShouldAddProduct()
        {
            // Arrange
            var newProduct = new Product { Id = 10, Name = "Product A", Price = 10.0 };

            // Act
            await _repository.Add(newProduct);

            // Assert
            var product = await _context.Products.FindAsync(10);
            product.Should().NotBeNull();
            product.Name.Should().Be("Product A");
            product.Price.Should().Be(10.0);
        }

        [Fact]
        public async Task Update_ShouldUpdateProduct()
        {
            // Arrange
            var initialProduct = new Product { Id = 5, Name = "Product A", Price = 10.0 };
            _context.Products.Add(initialProduct);
            await _context.SaveChangesAsync();

            var updatedProduct = new Product { Id = 5, Name = "Updated Product", Price = 15.0 };
            initialProduct.Name = updatedProduct.Name;
            initialProduct.Price = updatedProduct.Price;
            // Act
            await _repository.Update(initialProduct);

            // Assert
            var result = await _context.Products.FindAsync(5);
            result.Should().NotBeNull();
            result.Name.Should().Be("Updated Product");
            result.Price.Should().Be(15.0);
        }

        [Fact]
        public async Task Delete_ShouldRemoveProduct_WhenExists()
        {
            // Arrange
            var product = new Product { Id = 68, Name = "Product A", Price = 10.0 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.Delete(68);

            // Assert
            var deletedProduct = await _context.Products.FindAsync(68);
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldNotRemoveProduct_WhenNotExists()
        {
            // Arrange (no products in the context)

            // Act
            await _repository.Delete(1);

            // Assert 
            var product = await _context.Products.FindAsync(1);
            product.Should().BeNull();
        }

    }
}

