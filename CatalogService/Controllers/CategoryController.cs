using CatalogService.UseCases.CategoryUseCases.Commands.CreateCategory;
using CatalogService.UseCases.CategoryUseCases.Commands.DeleteCategoryByName;
using CatalogService.UseCases.CategoryUseCases.Commands.EditCategory;
using CatalogService.UseCases.CategoryUseCases.Queries.GetAllCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender) 
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createCategory")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            await _sender.Send(createCategoryCommand);
            return Ok("Category Created");
        }

        [HttpGet]
        [Route("getAllCategories")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _sender.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

        [HttpDelete]
        [Route("deleteCategoryByName")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> DeleteCategoryByName([FromBody] DeleteCategoryByNameCommand deleteCategoryByNameCommand)
        {
            await _sender.Send(deleteCategoryByNameCommand);
            return Ok("Category deleted");
        }

        [HttpPatch]
        [Route("editCategory")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand editCategoryCommand)
        {
            await _sender.Send(editCategoryCommand);
            return Ok("Category edited");
        }

    }
}
