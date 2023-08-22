using CatalogService.Entitites;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DataAccess;

public class MSSQLDataAccess : DbContext
{
    public MSSQLDataAccess(DbContextOptions<MSSQLDataAccess> options) : base(options) { }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }
}
