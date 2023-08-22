using CatalogService.Entitites;

namespace CatalogService.Repositories.Abstractions;

public interface ICategoryRepository
{
    public Task<Category> GetCategoryByCategoryName(string categoryName);
    public void CreateCategory(Category category);
    public void DeleteCategory(Category category);
    public Task<Category> GetCategoryById(int id);
    public Task<List<Category>> GetAllCategories();
}
