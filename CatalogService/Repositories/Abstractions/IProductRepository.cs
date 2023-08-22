using CatalogService.Entitites;
using MessagingHelper.Events;

namespace CatalogService.Repositories.Abstractions;

public interface IProductRepository
{
    public void CreateProduct(Product product);
    public Task<Product> GetProductById(int id);
    public void DeleteProduct(Product product);
    public List<Product> GetProductsBySubcategoryId(int subcatId);
    public Task<List<Product>> GetProductsByCategoryId(int categoryId);
    public Task EditProductsQuantityWithProductAfterOrderList(List<ProductAfterOrder> productAfterOrders);
}
