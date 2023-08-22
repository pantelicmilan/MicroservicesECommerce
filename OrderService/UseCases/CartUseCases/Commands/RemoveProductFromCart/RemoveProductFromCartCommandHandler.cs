using JwtAuthLibrary;
using MediatR;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.CartUseCases.Commands.RemoveProductFromCart;

public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveProductFromCartCommandHandler(
        IProductRepository productRepository, 
        ICartRepository cartRepository, 
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_httpContextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);

        var user = await _userRepository
            .GetUserByOriginalUserId(userId);
        if (user == null) throw new Exception("User not exist!");
        var product = await _productRepository
            .GetProductByOriginalProductId(request.OriginalProductId);
        if (product == null) throw new Exception("Product not exist!");
        var cart = await _cartRepository
            .GetCartWhereUserIdAndProductId(user.Id, product.Id);
        if (cart == null) throw new Exception("Item not found!");
        _cartRepository.DeleteCart(cart);
        await _unitOfWork.SaveChangesAsync();
    }
}
