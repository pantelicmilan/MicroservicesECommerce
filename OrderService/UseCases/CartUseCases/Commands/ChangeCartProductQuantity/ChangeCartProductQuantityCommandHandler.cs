using JwtAuthLibrary;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.CartUseCases.Commands.ChangeCartProductQuantity;

public class ChangeCartProductQuantityCommandHandler : IRequestHandler<ChangeCartProductQuantityCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangeCartProductQuantityCommandHandler(
        IUserRepository userRepository, 
        IProductRepository productRepository,
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(ChangeCartProductQuantityCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_httpContextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);

        var product = await _productRepository
            .GetProductByOriginalProductId(userId);
        if (product == null) throw new Exception("Product not found!");
        var user = await _userRepository
            .GetUserByOriginalUserId(userId);
        if (user == null) throw new Exception("User not found!");
        var cart = await _cartRepository
            .GetCartWhereUserIdAndProductId(user.Id, product.Id);
        if (cart == null) throw new Exception("Cart item not found");
        if(cart.Quantity != request.NewQuantity)
        {
            cart.Quantity = request.NewQuantity;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
