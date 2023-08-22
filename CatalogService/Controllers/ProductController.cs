using CatalogService.UseCases.ProductUseCases.Commands.CreateProduct;
using CatalogService.UseCases.ProductUseCases.Commands.DeleteProductById;
using CatalogService.UseCases.ProductUseCases.Commands.EditProduct;
using CatalogService.UseCases.ProductUseCases.Queries.GetProductById;
using CatalogService.UseCases.ProductUseCases.Queries.GetProductsByCategoryId;
using CatalogService.UseCases.ProductUseCases.Queries.GetProductsBySubcategoryId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createProduct")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand createProductCommand)
        {
            await _sender.Send(createProductCommand);
            return Ok("Product Created!");
        }

        [HttpPatch]
        [Route("editProduct")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> EditProduct([FromBody] EditProductCommand editProductCommand)
        {
            await _sender.Send(editProductCommand);
            return Ok("Product Edited");
        }

        [HttpDelete]
        [Route("deleteProductById/{id}")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> DeleteProductById([FromRoute] int id)
        {
            await _sender.Send(new DeleteProductByIdCommand { Id = id });
            return Ok("Product Deleted");
        }

        [HttpGet]
        [Route("getProductById/{id}")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _sender.Send(new GetProductByIdQuery { Id = id });
            return Ok(product);
        }

        [HttpGet]
        [Route("getProductsBySubCategoryId/{subCatId}")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetProductsBySubCategoryId([FromRoute] int subCatId)
        {
            var products = await _sender.Send(new GetProductsBySubcategoryIdQuery { SubcategoryId = subCatId });
            return Ok(products);
        }

        [HttpGet]
        [Route("getProductsByCategoryId/{categoryId}")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetProductsByCategoryId([FromRoute] int categoryId)
        {
            var products = await _sender.Send(new GetProductsByCategoryIdQuery { CategoryId = categoryId });
            return Ok(products);
        }
    }
}
