using MassTransit;
using MessagingHelper.Events;
using NotificationsService.Services.Abstractions;

namespace NotificationsService.Consumers;

public class VerificationCodeConsumer : IConsumer<VerficationCodeEvent>
{
    private readonly INotificationsSender _notificationSender;

    public VerificationCodeConsumer(INotificationsSender notificationsSender)
    {
        _notificationSender = notificationsSender;
    }

    public Task Consume(ConsumeContext<VerficationCodeEvent> context)
    {
        var message = context.Message;
        _notificationSender.Send(message.Email, "Your verification code!", message.VerificationCode);
        return Task.CompletedTask;
    }
}
