using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsMicroservices.Models;


namespace ProductsMicroservices.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIDAsync(int productId); // Added 'Async' suffix
        Task InsertProductAsync(Product product); // Added 'Async' suffix
        Task DeleteProductAsync(int productId); // Added 'Async' suffix
        Task UpdateProductAsync(Product product); // Added 'Async' suffix
        Task SaveAsync();
    }
}
