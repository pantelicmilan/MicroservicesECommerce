using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogService.Entitites;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int SubcategoryId { get; set; }
    [ForeignKey("SubcategoryId")]
    [JsonIgnore]
    public Subcategory Subcategory { get; set; }
    public int Quantity { get; set; }
    public IEnumerable<ProductImage> ProductImages { get; set; }
    public DateTime Posted { get; set; } = DateTime.Now;
}
