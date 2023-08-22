using OrderService.Entities;

namespace OrderService.Repositories.Abstractions;

public interface IProductRepository
{
    public void CreateProduct(Product product);
    public Task<Product> GetProductByOriginalProductId(int originalProductId);
    public void DeleteProduct(Product product);
    public Task<Product> GetProductById(int id);
    public Task<List<Product>> GetProductsWithProductIdList(List<int> productIdList);

}
