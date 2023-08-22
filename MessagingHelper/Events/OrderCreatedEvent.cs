using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingHelper.Events;

public class ProductAfterOrder
{
    public int Qtty { get; set; }
    public int OriginalProductId { get; set; }
}
public class OrderCreatedEvent
{
    public List<ProductAfterOrder> ProductsWithChangedQtty { get; set; }
}
