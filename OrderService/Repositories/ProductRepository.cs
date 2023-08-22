using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public ProductRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void CreateProduct(Product product)
    {
        _dataAccess.Products.Add(product);
    }

    public async Task<Product> GetProductByOriginalProductId(int originalProductId) 
    {
        var product = await _dataAccess.Products
            .FirstOrDefaultAsync(p => p.OriginalProductId == originalProductId);
        return product;
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _dataAccess.Products
            .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<List<Product>> GetProductsWithProductIdList(List<int> productIdList)
    {
        var products = await _dataAccess.Products
        .Where(p => productIdList.Contains(p.Id))
        .ToListAsync();
        return products;
    }

    public void DeleteProduct(Product product)
    {
        _dataAccess.Products.Remove(product);
    }

}
