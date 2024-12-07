using Ozon.Application.Abstractions;
using Ozon.Core.Models;
using Ozon.DataAccess.Context;

namespace Ozon.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(Guid id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            var existingProduct = _context.Products.Find(product.Id);
    
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Продукт с ID {product.Id} не найден.");
            }
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}