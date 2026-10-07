using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;
namespace EcommerceStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
        b.Entity<Order>().Property(o => o.Total).HasPrecision(18, 2);
        b.Entity<OrderItem>().Property(i => i.UnitPrice).HasPrecision(18, 2);
        b.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Wireless Headphones", Category = "Audio", Price = 7500, Stock = 25, Description = "Over-ear Bluetooth headphones with a long-lasting battery." },
            new Product { Id = 2, Name = "Mechanical Keyboard", Category = "Accessories", Price = 12500, Stock = 15, Description = "Compact keyboard with tactile switches and backlight." },
            new Product { Id = 3, Name = "Wireless Mouse", Category = "Accessories", Price = 2800, Stock = 40, Description = "Quiet, ergonomic mouse with a USB receiver." },
            new Product { Id = 4, Name = "USB-C Hub", Category = "Accessories", Price = 4200, Stock = 30, Description = "6-in-1 hub with HDMI, USB 3.0 and card reader." },
            new Product { Id = 5, Name = "Laptop Backpack", Category = "Bags", Price = 5600, Stock = 20, Description = "Water-resistant backpack that fits up to 15.6 inch laptops." },
            new Product { Id = 6, Name = "Portable Speaker", Category = "Audio", Price = 6200, Stock = 18, Description = "Compact Bluetooth speaker with clear sound." });
    }
}
