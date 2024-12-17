using Microsoft.EntityFrameworkCore;
using Ozon.Core.Models;
using Ozon.DataAccess.Abstractions;
using Ozon.DataAccess.Context;

namespace Ozon.DataAccess.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesWithProductsAsync()
        {
            return await _context.Warehouses.Include(w => w.Products).ToListAsync();
        }
    }
}