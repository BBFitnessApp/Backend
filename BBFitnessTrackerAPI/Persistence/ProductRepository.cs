using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Persistence
{
    internal class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public void Delete(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FirstAsync(p => p.Id == productId);
        }

        public async Task<Product> GetProductByBarcode(string barcode)
        {
            return await _dbContext.Products.FirstAsync(p => p.Barcode == barcode);
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}