using OrderService.Entities;

namespace OrderService.Repositories.Abstractions;

public interface ICartRepository
{
    public void AddToCart(CartItem cart);
    public Task<List<CartItem>> GetCartsByUserId(int userId);
    public void DeleteCart(CartItem cart);
    public Task<CartItem> GetCartById(int id);
    public Task<CartItem> GetCartWhereUserIdAndProductId(int userId, int productId);
}
