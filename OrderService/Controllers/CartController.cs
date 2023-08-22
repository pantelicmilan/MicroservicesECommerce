using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.UseCases.CartUseCases.Commands.AddProductToCart;
using OrderService.UseCases.CartUseCases.Commands.ChangeCartProductQuantity;
using OrderService.UseCases.CartUseCases.Commands.RemoveProductFromCart;
using OrderService.UseCases.CartUseCases.Queries.GetCartByOriginalUserId;
using OrderService.UseCases.ProductUseCases.Commands.DeleteProductById;
using OrderService.UseCases.ProductUseCases.Commands.EditProduct;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ISender _sender;
        
        public CartController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("addProductToCart")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> AddProductToCart(AddProductToCartCommand addProductToCartCommand)
        {
            await _sender.Send(addProductToCartCommand);
            return Ok("Product Add in Cart");
        }

        [HttpDelete]
        [Route("deleteProductFromCart")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> DeleteProductFromCart(
            RemoveProductFromCartCommand removeProductFromCartCommand)
        {
            await _sender.Send(removeProductFromCartCommand);
            return Ok("Product Deleted from Cart");
        }

        [HttpPatch]
        [Route("editProductFromCart")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> EditProductFromCart(
            ChangeCartProductQuantityCommand changeCartProductQuantityCommand)
        {
            await _sender.Send(changeCartProductQuantityCommand);
            return Ok("Product Edited in Cart!");
        }

        [HttpGet]
        [Route("getCartByOriginalUserId")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> GetCartByOriginalUserId()
        {
            var cart = await _sender.Send(new GetCartByOriginalUserIdQuery());
            return Ok(cart);
        }

    }
}
