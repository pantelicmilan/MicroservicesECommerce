using MassTransit;
using MediatR;
using MessagingHelper.Events;
using OrderService.Repositories.Abstractions;
using OrderService.UseCases.UserUseCases.Commands.DeleteUserByOriginalUserId;

namespace OrderService.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly ISender _sender;

    public UserDeletedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var message = context.Message;
        await _sender.Send(new DeleteUserByOriginalUserIdCommand { 
            OriginalUserId = message.OriginalUserId
        });
    }
}
