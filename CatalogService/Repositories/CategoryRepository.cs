using CatalogService.DataAccess;
using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CatalogService.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private MSSQLDataAccess _dataAcces;

    public CategoryRepository(MSSQLDataAccess dataAccess)
    {
        _dataAcces = dataAccess;
    }

    public void CreateCategory(Category category)
    {
        _dataAcces.Categories.Add(category);
    }

    public async Task<Category> GetCategoryByCategoryName(string categoryName)
    {
        var category = await _dataAcces.Categories
            .Include(cat=>cat.Subcategories)
            .ThenInclude(sub=>sub.Products)
            .ThenInclude(prod=>prod.ProductImages)
            .FirstOrDefaultAsync(cat => cat.Name == categoryName);
        return category;
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _dataAcces.Categories
            .Include(cat=>cat.Subcategories)
            .FirstOrDefaultAsync(cat => cat.Id == id);
        return category;
    }

    public void DeleteCategory(Category category)
    {
        _dataAcces.Categories.Remove(category);
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var categories = _dataAcces.Categories
            .Include(c=> c.Subcategories)
            .ThenInclude(sub=>sub.Products)
            .ThenInclude(prod=>prod.ProductImages).ToList();
        return categories;
    }
}
