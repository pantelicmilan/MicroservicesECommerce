using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderService.Entities;

public class CartItem
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    [JsonIgnore]
    [ForeignKey("UserId")]
    public User User { get; set; }
}
