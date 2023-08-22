using CatalogService.Controllers;
using CatalogService.DataAccess;
using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories;

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public SubCategoryRepository(MSSQLDataAccess dataAccess)
    { 
        _dataAccess = dataAccess;
    }

    public void CreateSubCategory(Subcategory subcategory)
    {
        _dataAccess.Subcategories.Add(subcategory);
    }

    public async Task<Subcategory> GetSubcategoryByName(string name)
    {
        var subcategory = await _dataAccess.Subcategories
            .Include(sub => sub.Products)
            .ThenInclude(prod=>prod.ProductImages)
            .FirstOrDefaultAsync(x => x.Name == name);
        return subcategory;
    }

    public async Task<Subcategory> GetSubcategoryById(int id)
    {
        var subcategory = await _dataAccess.Subcategories
            .Include(sub=> sub.Products)
            .ThenInclude(prod=>prod.ProductImages)
            .FirstOrDefaultAsync(s => s.Id == id);
        return subcategory;
    }

    public void DeleteSubcategory(Subcategory subcategory)
    {
        _dataAccess.Subcategories.Remove(subcategory);
    }
}
