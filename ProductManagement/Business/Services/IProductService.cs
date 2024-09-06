using Business.Models;

namespace Business.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto?> GetById(int id);
        Task Add(ProductDetailsModel newProduct);
        Task Update(int id, ProductDetailsModel updatedProduct);
        Task Delete(int id);
    }
}
