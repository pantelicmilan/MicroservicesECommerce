using MassTransit;
using MediatR;
using MessagingHelper.Events;
using Microsoft.Identity.Client;
using OrderService.UseCases.UserUseCases.Commands.CreateUser;

namespace OrderService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ISender _sender;

    public UserCreatedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;

        await _sender.Send(new CreateUserCommand
        {
            Email = message.Email,
            OriginalUserId = message.OriginalUserId,
            Username = message.Username
        });
    }
}
