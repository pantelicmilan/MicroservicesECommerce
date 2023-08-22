using OrderService.Entities;

namespace OrderService.Repositories.Abstractions;

public interface IOrderRepository
{
    public void CreateOrder(List<CartItem> cartItems, Order order);
}
