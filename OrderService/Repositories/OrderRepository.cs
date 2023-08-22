using OrderService.DataAccess;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public OrderRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;        
    }

    public void CreateOrder(List<CartItem> cartItems, Order order)
    {
        var orderItemList = new List<OrderItem>();
        foreach (var cartItem in cartItems)
        {
            orderItemList.Add(new OrderItem
            {
                QuantityOrdered = cartItem.Quantity,
                ProductId = cartItem.ProductId,
            });
            _dataAccess.Remove(cartItem);
        }
        order.OrderItems = orderItemList;
        order.Status = OrderStatus.Ordered;
        _dataAccess.Orders.Add(order);
    }

}
