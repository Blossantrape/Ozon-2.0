using System.ComponentModel.DataAnnotations;

namespace Ozon.Core.Models
{
    // Полный заказ.
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        
        // Внешний ключ.
        public Guid Uuid { get; set; } = Guid.NewGuid(); 
        
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        [Range(0, double.MaxValue, ErrorMessage = "Сумма заказа не может быть отрицательной.")]
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.New; // Статус заказа
    }

    public enum OrderStatus
    {
        New,
        Processing,
        Shipped,
        Delivered,
        Canceled
    }
}