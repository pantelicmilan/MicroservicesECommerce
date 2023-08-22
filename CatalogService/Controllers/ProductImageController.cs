using CatalogService.UseCases.ProductImageUseCases.Commands.DeleteProductImageById;
using CatalogService.UseCases.ProductImageUseCases.Commands.UploadProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductImageController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("uploadProductImages")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> UploadProductImages([FromForm] List<IFormFile> images, [FromForm] int productId)
        {
            await _sender.Send(new UploadProductImagesCommand { Images = images, ProductId = productId });
            return Ok("Uploaded");
        }

        [HttpDelete]
        [Route("deleteProductImages")]
        [Authorize(Policy = "RequireUserHaveAdminRole")]
        public async Task<IActionResult> DeleteProductImages([FromBody] List<int> productImagesIds)
        {
            await _sender.Send(new DeleteProductImagesByIdCommand { ProductImageIdList = productImagesIds });
            return Ok("Images deleted");
        }
    }
}
