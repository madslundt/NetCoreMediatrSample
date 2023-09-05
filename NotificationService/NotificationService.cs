namespace NotificationService;

public class NotificationService : INotificationService
{
    public Task SendEmail(string title, string message, string email)
    {
        Console.WriteLine("Sending email notification...");

        return Task.CompletedTask;
    }

    public Task SendPushNotification(string title, string message, string recipient)
    {
        Console.WriteLine("Sending push notification...");

        return Task.CompletedTask;
    }
}