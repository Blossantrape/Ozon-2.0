using Microsoft.EntityFrameworkCore;
using Ozon.Core.Models;
using Ozon.DataAccess.Abstractions;
using Ozon.DataAccess.Context;

namespace Ozon.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid uuid)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == uuid);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        { 
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uuid)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == uuid);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}