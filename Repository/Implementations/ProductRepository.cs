using Microsoft.EntityFrameworkCore;
using ProductsMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsMicroservices.Interfaces;
using ProductsMicroservices.ProductDbContext;


namespace ProductsMicroservices.Implementations
{
        public class ProductRepository : IProductRepository
        {
            private readonly ProductContext _dbContext;

            public ProductRepository(ProductContext dbContext)
            {
                _dbContext = dbContext;
            }
        public async Task DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync(); // Save changes asynchronously
            }
        }

        public async Task<Product> GetProductByIDAsync(int productId)
        {
            return await _dbContext.Products.FindAsync(productId);
        }


        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task InsertProductAsync(Product product)
        {
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously
        }
        public async Task SaveAsync() // Added for consistency (optional)
        {
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously
        }

        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Products.Attach(product); // Attach the product to the context
            _dbContext.Entry(product).State = EntityState.Modified; // Set state after attaching
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously
        }
    }
}
