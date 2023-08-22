using CatalogService.DataAccess;
using CatalogService.Entitites;
using CatalogService.UseCases.SubCategoryUseCases.Commands.CreateSubCategory;
using CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryById;
using CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryByName;
using CatalogService.UseCases.SubCategoryUseCases.Commands.EditSubCategory;
using CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryById;
using CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public SubCategoryController(ISender sender, MSSQLDataAccess dataAccess)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createSubcategory")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> CreateSubcategory([FromBody]CreateSubCategoryCommand createSubCategoryCommand)
        {
            await _sender.Send(createSubCategoryCommand);
            return Ok("Subcategory Created");
        }

        [HttpPatch]
        [Route("editSubcategory")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> EditSubcategory([FromBody]EditSubCategoryCommand editSubCategoryCommand)
        {
            await _sender.Send(editSubCategoryCommand);
            return Ok("Subcategory edited");
        }

        [HttpGet]
        [Route("getSubcategoryByName/{name}")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetSubcategoryByName([FromRoute] string name)
        {
            var subCategory = await _sender.Send( new GetSubCategoryByNameQuery { SubcategoryName = name });
            return Ok(subCategory);
        }

        [HttpGet]
        [Route("getSubcategoryById/{id}")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetSubcategoryByName([FromRoute] int id)
        {
            var subCategory = await _sender.Send(new GetSubCategoryByIdQuery { Id = id });
            return Ok(subCategory);
        }

        [HttpDelete]
        [Route("deleteSubcategoryById/{id}")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> DeleteSubcategoryById([FromRoute] int id)
        {
            await _sender.Send(new DeleteSubCategoryByIdCommand { Id = id });
            return Ok("Subcategory deleted");
        }

        [HttpDelete]
        [Route("deleteSubcategoryByName/{name}")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> DeleteSubcategoryByName([FromRoute] string name)
        {
            await _sender.Send(new DeleteSubCategoryByNameCommand { SubCategoryName = name });
            return Ok("Subcategory deleted");
        }

    }
}
