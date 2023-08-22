using CatalogService.Entitites;

namespace CatalogService.Repositories.Abstractions;

public interface IProductImageRepository
{
    public void UploadProductImage(string imageUrl, int productId);
    public void DeleteProductImage(ProductImage productImage);
    public Task<List<ProductImage>> IfAllIdsExistReturnProductImageListAsync(List<int> productImageIds);
}
