using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.DataAccess;

public class MSSQLDataAccess : DbContext
{
    public MSSQLDataAccess(DbContextOptions<MSSQLDataAccess> options) : base(options) { }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CartItem>()
            .HasIndex(c => new { c.UserId, c.ProductId })
            .IsUnique();

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
        .HasCheckConstraint("CHK_OrderStatus", "[Status] IN ('Ordered', 'Processed', 'Delivered')");
    }
}
