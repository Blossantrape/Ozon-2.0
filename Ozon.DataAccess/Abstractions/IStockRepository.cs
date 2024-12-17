using Ozon.Core.Models;

namespace Ozon.DataAccess.Abstractions;

public interface IStockRepository
{
    Task<IEnumerable<Warehouse>> GetAllWarehousesWithProductsAsync();
}