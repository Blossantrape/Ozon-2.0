using Microsoft.EntityFrameworkCore;
using Ozon.Core.Models;

namespace Ozon.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь Order -> OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)  // Указываем, что OrderId ссылается на Uuid в Order
                .OnDelete(DeleteBehavior.Cascade); // Удаление позиций при удалении заказа

            // Связь OrderItem -> Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Предотвращение удаления продукта, если он используется

            // Связь Order -> User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Генерация UUID для OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Uuid)
                    .HasDefaultValueSql("gen_random_uuid()") // Использует функцию PostgreSQL для генерации UUID
                    .IsRequired();
                entity.HasIndex(e => e.Uuid).IsUnique(); // Уникальный индекс
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}