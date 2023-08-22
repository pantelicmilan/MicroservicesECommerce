using MediatR;
using MessagingHelper.Events;

namespace CatalogService.UseCases.ProductUseCases.Commands.EditQttyForProductList;

public class EditQttyForProductListCommand : IRequest
{
    public List<ProductAfterOrder> ProductAfterOrders { get; set; }
}
