using System.ComponentModel.DataAnnotations;

namespace Ozon.Core.Models
{
    // Позиция заказа.
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        // Внешний ключ.
        public Guid Uuid { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0.")]
        public int Quantity { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0.")]
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
}