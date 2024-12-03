namespace Ozon.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}