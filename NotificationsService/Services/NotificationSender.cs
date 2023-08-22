using NotificationsService.Services.Abstractions;
using System.Net.Mail;

namespace NotificationsService.Services;

public class NotificationSender : INotificationsSender
{
    public void Send(string clientMail, string subject, string body)
    {
        Console.WriteLine("Send To:");
        Console.WriteLine($"<{clientMail}>");
        Console.WriteLine("Subject:");
        Console.WriteLine($"<{subject}>");
        Console.WriteLine("Body:");
        Console.WriteLine($"<{body}>");
    }
}
