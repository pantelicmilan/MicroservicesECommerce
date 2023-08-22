namespace NotificationsService.Services.Abstractions;

public interface INotificationsSender
{
    //can implement with smtp integration
    public void Send(string clientIdentificator, string title, string content);
}
