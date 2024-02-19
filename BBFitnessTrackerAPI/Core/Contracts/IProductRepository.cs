using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Core.Contracts
{
    public interface IProductRepository
    {
        Task<Product> GetProductByBarcode(string barcode);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
