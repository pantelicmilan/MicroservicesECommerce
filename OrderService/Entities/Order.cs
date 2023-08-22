using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Entities;

public enum OrderStatus
{
    Ordered ,
    Processed ,
    Delivered ,
}

public class Order
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User OrderCreator { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;
}
