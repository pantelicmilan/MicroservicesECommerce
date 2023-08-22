using AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess;

public class MSSQLDataAccess : DbContext
{
    public MSSQLDataAccess(DbContextOptions<MSSQLDataAccess> options) : base(options) {}
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
               .Property(u => u.Role)
               .HasConversion<string>();
    }
}
