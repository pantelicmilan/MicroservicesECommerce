using CatalogService.DataAccess;
using CatalogService.Repositories.Abstractions;
using CatalogService.Entitites;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories;

public class ProductImageRepository: IProductImageRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public ProductImageRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void UploadProductImage(string imageUrl, int productId)
    {
        _dataAccess.ProductImages
            .Add(new ProductImage { ImageUrl = imageUrl, ProductId = productId});
    }

    public void DeleteProductImage(ProductImage productImage)
    {
        _dataAccess.ProductImages.Remove(productImage);
    }

    public async Task<List<ProductImage>> IfAllIdsExistReturnProductImageListAsync(List<int> productImageIds)
    {
        if (productImageIds == null || productImageIds.Count == 0)
        {
            return null; 
        }

        var uniqueInputIds = new HashSet<int>(productImageIds);

        var existingProductImages = await _dataAccess.ProductImages
            .Where(e => uniqueInputIds.Contains(e.Id))
            .ToListAsync();

        if (existingProductImages.Count == uniqueInputIds.Count)
        {
            return existingProductImages; // Svi ID-jevi se podudaraju
        }
        else
        {
            return null; // Barem jedan ID se ne podudara
        }
    }

}
