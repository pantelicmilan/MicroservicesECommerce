using MassTransit;
using MediatR;
using MessagingHelper.Events;
using OrderService.UseCases.ProductUseCases.Commands.DeleteProductById;
using OrderService.UseCases.ProductUseCases.Queries.GetProductById;

namespace OrderService.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly ISender _sender;

    public ProductDeletedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        await _sender.Send(new DeleteProductByOriginalProductIdCommand {
            OriginalProductId = context.Message.ProductId 
        });
    }
}
