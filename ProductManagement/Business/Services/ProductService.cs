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

        public async Task Add(ProductDetailsModel newProduct)
        {
            try
            {
                var validation = Validate(newProduct);
                if (!validation.IsValid)
                {
                    var message = string.Join(",/n ", validation.Errors);
                    throw new ArgumentException(message);
                }
                await _productRepository.Add(new Product { Name = newProduct.Name, Price = newProduct.Price.Value });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid id value.");
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
                if (id <= 0) throw new ArgumentOutOfRangeException("Invalid value for id.");
                var product = await _productRepository.GetById(id);
                if (product == null) return null;
                return new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(int id, ProductDetailsModel updatedProduct)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Invalid value for id.");
                var product = await _productRepository.GetById(id);
                if (product == null) throw new InvalidOperationException("There's no product associated with the id.");
                var validation = Validate(updatedProduct, true);
                if (!validation.IsValid)
                {
                    var message = string.Join(",/n ", validation.Errors);
                    throw new ArgumentException(message);
                }
                if (updatedProduct.Name != product.Name || updatedProduct.Price != product.Price)
                {
                    if (updatedProduct.Name != null && updatedProduct.Name != product.Name) product.Name = updatedProduct.Name;
                    if (updatedProduct.Price != null && updatedProduct.Price != product.Price) product.Price = updatedProduct.Price.Value;
                    await _productRepository.Update(product);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ValidationResult Validate(ProductDetailsModel productDetails, bool isUpdate = false)
        {
            bool valid = true;
            List<string> errors = [];

            if (!isUpdate)
            {
                if (string.IsNullOrEmpty(productDetails.Name))
                {
                    errors.Add("Invalid product name.");
                    valid = false;
                }
            }
            else
            {
                if (productDetails.Name != null && string.IsNullOrEmpty(productDetails.Name))
                {
                    errors.Add("Invalid product name.");
                    valid = false;
                }
            }

            if (!isUpdate || productDetails.Price != null)
            {
                if (productDetails.Price == null || productDetails.Price < 0.0)
                {
                    errors.Add("Invalid product price.");
                    valid = false;
                }
            }

            return new ValidationResult { IsValid = valid, Errors = errors };
        }

    }
}
