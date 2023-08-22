using JwtAuthLibrary;
using MediatR;
using MessagingHelper.Events;
using OrderService.EventBus;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.OrderUseCases.Commands.SubmitOrder;

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IEventBus _eventBus;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubmitOrderCommandHandler(
        IUserRepository userRepository, 
        ICartRepository cartRepository, 
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IEventBus eventBus,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _eventBus = eventBus;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_httpContextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);

        var user = await _userRepository
            .GetUserByOriginalUserId(userId);
        if (user == null) throw new Exception("User not found");
        var cart = await _cartRepository.GetCartsByUserId(user.Id);
        if (cart == null) throw new Exception("Cart not found");
        _orderRepository.CreateOrder(cart, new Entities.Order() { UserId = user.Id });
        List<int> productIdList = new List<int>();
        foreach(var cartItem in cart)
        {
            productIdList.Add(cartItem.ProductId);
        }
        var productList = await _productRepository.GetProductsWithProductIdList(productIdList);
        if (productList == null) throw new Exception("Products not found");
        foreach (var product in productList)
        {
            foreach (var cartItem in cart)
            {
                if(product.Id == cartItem.ProductId)
                {
                    if(product.Qtty - cartItem.Quantity < 0)
                    {
                        throw new Exception($"Product {product.ProductName} out of stock, " +
                            $"left {product.Qtty}, you have {cartItem.Quantity} in cart! ");
                    }

                    product.Qtty -= cartItem.Quantity;
                }
            }
        }
        await _unitOfWork.SaveChangesAsync();

        List<ProductAfterOrder> productsAfterOrder = new List<ProductAfterOrder>();
        foreach(var product in productList)
        {
            productsAfterOrder.Add(new ProductAfterOrder { 
                OriginalProductId = product.OriginalProductId, 
                Qtty = product.Qtty 
            });
        }

        await _eventBus.PublishAsync<OrderCreatedEvent>(new OrderCreatedEvent
        {
            ProductsWithChangedQtty = productsAfterOrder
        });

    }
}
