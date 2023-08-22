using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string ProductName { get; set; }
    public int Qtty { get; set; }
    public decimal Price { get; set; }
    public int OriginalProductId { get; set; }
}
