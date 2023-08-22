using CatalogService.UseCases.ProductUseCases.Commands.EditProduct;
using CatalogService.UseCases.ProductUseCases.Commands.EditQttyForProductList;
using CatalogService.UseCases.ProductUseCases.Queries.GetProductById;
using MassTransit;
using MediatR;
using MessagingHelper.Events;

namespace CatalogService.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ISender _sender;

    public OrderCreatedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        await _sender.Send(new EditQttyForProductListCommand {
            ProductAfterOrders = message.ProductsWithChangedQtty 
        });
    }
}
