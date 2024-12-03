using Ozon.Application.Abstractions;
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

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrderById(Guid id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Guid id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}