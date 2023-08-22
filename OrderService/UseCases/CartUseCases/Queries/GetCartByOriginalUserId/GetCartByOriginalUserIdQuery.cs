using MediatR;
using OrderService.Entities;

namespace OrderService.UseCases.CartUseCases.Queries.GetCartByOriginalUserId;

public class GetCartByOriginalUserIdQuery : IRequest<List<CartItem>> {}
