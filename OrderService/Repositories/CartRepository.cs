using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OrderService.DataAccess;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.Repositories;

public class CartRepository : ICartRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public CartRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void AddToCart(CartItem cart)
    {
        _dataAccess.CartItems.Add(cart);
    }

    public async Task<List<CartItem>> GetCartsByUserId(int userId)
    {
        var cart = await _dataAccess.CartItems
            .Include(c=>c.Product)
            .Where(c => c.UserId == userId).ToListAsync();
        return cart;
    }

    public async Task<CartItem> GetCartWhereUserIdAndProductId(int userId, int productId)
    {
        var cart =  await _dataAccess.CartItems
            .Include(c => c.Product)
            .SingleOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);
        return cart;
    }

    public void DeleteCart(CartItem cart)
    {
        _dataAccess.CartItems.Remove(cart);
    }

    public async Task<CartItem> GetCartById(int id)
    {
        var cart = await _dataAccess.CartItems
            .Include(c=> c.Product)
            .FirstOrDefaultAsync(c=>c.Id == id);
        return cart;
    }

    
}
