using MassTransit;
using MediatR;
using MessagingHelper.Events;
using OrderService.UseCases.UserUseCases.Commands.EditUser;

namespace OrderService.Consumers;

public class UserEditedConsumer : IConsumer<UserEditedEvent>
{
    private readonly ISender _sender;

    public UserEditedConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UserEditedEvent> context)
    {
        var message = context.Message;

        await _sender.Send(new EditUserCommand { 
            Email = message.Email, 
            OriginalUserId = message.OriginalUserId, 
            UserName = message.UserName
        });
    }
}
