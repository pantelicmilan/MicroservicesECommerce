﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Entities;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int QuantityOrdered { get; set; }
    public int OrderId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
}
