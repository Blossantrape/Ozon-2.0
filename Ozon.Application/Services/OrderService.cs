using Microsoft.EntityFrameworkCore;
using Ozon.Core.Abstractions;
using Ozon.Core.Models;
using Ozon.DataAccess.Context;

namespace Ozon.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(order.Id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Заказ с ID {order.Id} не найден.");
            }

            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}