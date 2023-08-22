using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<List<Category>> { }
