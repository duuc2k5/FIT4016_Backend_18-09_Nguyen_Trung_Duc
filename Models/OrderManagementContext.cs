using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Models.Entities;

namespace OrderManagementApp.Models
{
    public class OrderManagementContext : DbContext
    {
        public OrderManagementContext(DbContextOptions<OrderManagementContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Description)
                    .HasMaxLength(500);
                entity.Property(e => e.Price)
                    .HasPrecision(10, 2);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                
                entity.Property(e => e.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(20);
                
                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(100);
                
                // Unique constraint on OrderNumber and CustomerEmail
                entity.HasIndex(e => e.OrderNumber)
                    .IsUnique();
                
                entity.HasIndex(e => e.CustomerEmail);
                
                // Foreign key relationship
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
