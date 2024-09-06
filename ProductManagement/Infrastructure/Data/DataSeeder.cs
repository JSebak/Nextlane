using Infrastructure.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ProductManagementDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(ProductManagementDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            if (_context.Products.Any())
            {
                return;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var products = new List<Product>
                    {
                        new Product{ Name = "Producto1",Price = 9.2},
                        new Product{ Name = "Producto2",Price = 29.12},
                        new Product{ Name = "Producto3",Price = 15.0},
                    };


                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                    await transaction.RollbackAsync();
                    throw new Exception("Database seeding failed. The transaction has been rolled back.", ex);
                }
            }
        }
    }
}
