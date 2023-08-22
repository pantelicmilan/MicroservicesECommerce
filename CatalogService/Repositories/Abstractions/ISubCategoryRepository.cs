using CatalogService.Entitites;

namespace CatalogService.Repositories.Abstractions;

public interface ISubCategoryRepository
{
    public void CreateSubCategory(Subcategory subcategory);
    public Task<Subcategory> GetSubcategoryByName(string name);
    public Task<Subcategory> GetSubcategoryById(int id);
    public void DeleteSubcategory(Subcategory subcategory);
}
