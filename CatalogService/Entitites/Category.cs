using System.ComponentModel.DataAnnotations;

namespace CatalogService.Entitites;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Subcategory> Subcategories { get; set; }
}
