using CatalogService.DataAccess;
using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using CatalogService.UseCases.ProductUseCases.Queries.GetProductsBySubcategoryId;
using MessagingHelper.Events;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories;

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

    public void DeleteProduct(Product product)
    {
        _dataAccess.Products.Remove(product);
    }

    public async Task<Product> GetProductById(int id)
    {
        var product =  await _dataAccess.Products
            .Include(pro=>pro.ProductImages)
            .FirstOrDefaultAsync(pro => pro.Id == id);
        return product;
    }

    public List<Product> GetProductsBySubcategoryId(int subcatId)
    {
        var products = _dataAccess.Products
            .Include(pro=>pro.ProductImages)
            .Where(pro=> pro.SubcategoryId == subcatId).ToList();
        return products;
    }

    public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
    {
        var category = await _dataAccess.Categories
            .Include(cat=>cat.Subcategories)
            .ThenInclude(subcat=> subcat.Products)
            .ThenInclude(prod=>prod.ProductImages)
            .FirstOrDefaultAsync(cat => cat.Id == categoryId);
        if (category != null)
        {
            var products = category.Subcategories
                                  .SelectMany(subcat => subcat.Products)
                                  .ToList();
            return products;
        }
        return null;
    }

    public async Task EditProductsQuantityWithProductAfterOrderList(
        List<ProductAfterOrder> productAfterOrders)
    {
        List<int> productIdList = productAfterOrders.Select(order => order.OriginalProductId).ToList();

        var products = await _dataAccess.Products
         .Where(p => productIdList.Contains(p.Id))
         .ToListAsync();

        foreach (var order in productAfterOrders)
        {
            var product = products.FirstOrDefault(p => p.Id == order.OriginalProductId);
            if (product != null)
            {
                product.Quantity = order.Qtty;
            }
        }
    }

}
