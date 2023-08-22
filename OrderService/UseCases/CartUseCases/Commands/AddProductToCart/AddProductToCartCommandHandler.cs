using MediatR;
using OrderService.Repositories.Abstractions;
using OrderService.Entities;
using JwtAuthLibrary;

namespace OrderService.UseCases.CartUseCases.Commands.AddProductToCart;

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor; 

    public AddProductToCartCommandHandler(
        ICartRepository cartRepository, 
        IUserRepository userRepository, 
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_httpContextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);

        var product = await _productRepository
            .GetProductByOriginalProductId(request.OriginalProductId);
        if (product == null) throw new Exception("Product does not exist!");
        if (product.Qtty < request.Quantity) throw new Exception("We do not have this amount in storage!");
        var user = await _userRepository
            .GetUserByOriginalUserId(userId);
        var cart = await _cartRepository.GetCartWhereUserIdAndProductId(user.Id, product.Id);
        if (cart != null) throw new Exception("Product already added in cart, you can change amount only!");
        if (user == null) throw new Exception("User does not exist!");
        _cartRepository.AddToCart(new CartItem { 
            ProductId = product.Id, 
            UserId = user.Id, 
            Quantity = request.Quantity 
        });
        await _unitOfWork.SaveChangesAsync();
    }
}
