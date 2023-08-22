using MassTransit;
using MediatR;
using MessagingHelper.Events;
using OrderService.Repositories.Abstractions;
using OrderService.UseCases.ProductUseCases.Commands.CreateProduct;

namespace OrderService.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ISender _sender;

    public ProductCreatedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var message = context.Message;
        await _sender.Send(new CreateProductCommand { 
            OriginalProductId = message.ProductId,
            Price = message.Price, 
            ProductName = message.ProductName, 
            Qtty = message.Qtty
        });
    }
}
