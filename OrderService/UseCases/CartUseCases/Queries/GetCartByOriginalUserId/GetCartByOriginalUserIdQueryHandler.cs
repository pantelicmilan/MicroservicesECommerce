using JwtAuthLibrary;
using MediatR;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.CartUseCases.Queries.GetCartByOriginalUserId;

public class GetCartByOriginalUserIdQueryHandler : IRequestHandler<GetCartByOriginalUserIdQuery, List<CartItem>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCartByOriginalUserIdQueryHandler(
        ICartRepository cartRepository, 
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<CartItem>> Handle(GetCartByOriginalUserIdQuery request, CancellationToken cancellationToken)
    {
        var jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_httpContextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);

        var user = await _userRepository.GetUserByOriginalUserId(userId);
        if (user == null) throw new Exception("User not found!");
        var cart = await _cartRepository.GetCartsByUserId(user.Id);
        if (cart == null) throw new Exception("Cart not found!");
        return cart;
    }
}
