using Ozon.Core.Models;

namespace Ozon.DataAccess.Abstractions;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid uuid);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid uuid);
}