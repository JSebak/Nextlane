using Business.Models;
using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Task Add(ProductDetailsModel newProduct)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "Invalid id value.");
                }

                var product = await _productRepository.GetById(id);
                if (product == null)
                {
                    throw new InvalidOperationException($"No product found with id {id}");
                }

                await _productRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            try
            {
                var products = (await _productRepository.GetAll()).Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price }).ToList();
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDto?> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetById(id);
                if (product == null) return null;
                return new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task Update(int id, ProductDetailsModel updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
