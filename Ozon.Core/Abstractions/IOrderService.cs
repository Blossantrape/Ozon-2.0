using Ozon.Core.Models;

namespace Ozon.Core.Abstractions
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(Guid id);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Guid id);
    }
}