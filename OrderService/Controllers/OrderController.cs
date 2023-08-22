using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.UseCases.OrderUseCases.Commands.SubmitOrder;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;    
        }

        [HttpPost]
        [Route("submitOrder")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> SubmitOrder()
        {
            await _sender.Send(new SubmitOrderCommand());
            return Ok("Order Submited");
        }

    }
}
