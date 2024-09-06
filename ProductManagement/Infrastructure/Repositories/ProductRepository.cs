﻿using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ProductManagementDbContext _context;
        public ProductRepository(ProductManagementDbContext context)
        {
            _context = context;
        }

        public async Task Add(Product newEntity)
        {
            try
            {
                _context.Products.Add(newEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Invalid id value");
                return await _context.Products.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(int id, Product updatedEntity)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null) throw new ArgumentException($"There's no product with the id: {id}");
                _context.Products.Update(updatedEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
