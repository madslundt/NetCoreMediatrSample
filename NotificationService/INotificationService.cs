namespace NotificationService;

public interface INotificationService
{
    Task SendEmail(string title, string message, string email);
    Task SendPushNotification(string title, string message, string recipient);
}