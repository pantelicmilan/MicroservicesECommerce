using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogService.Entitites;

public class Subcategory
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
