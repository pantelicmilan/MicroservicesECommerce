using MassTransit;
using MediatR;
using MessagingHelper.Events;
using OrderService.UseCases.ProductUseCases.Commands.EditProduct;

namespace OrderService.Consumers;

public class ProductEditedConsumer : IConsumer<ProductEditedEvent>
{
    private readonly ISender _sender;
    
    public ProductEditedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ProductEditedEvent> context)
    {
        var message = context.Message;

        await _sender.Send(new EditProductCommand
        {
            OriginalProductId = message.ProductId,
            ProductName = message.Name,
            Qtty = message.Quantity,
            Price = message.Price
        });
    }
}
