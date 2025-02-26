using Notification.Api.Models;

namespace Notification.Api.Services;

public class NotificationManager
{
    private readonly List<INotification> _notifications = [];

    public void Subscribe(INotification notification)
    {
        _notifications.Add(notification);
    }

    public void Unsubscribe(INotification notification)
    {
        _notifications.Remove(notification);
    }

    public async Task Notify(SendNotificationRequest request)
    {
        foreach (var notification in _notifications)
        {
            await notification.NotifyAsync(request);
        }
    }
}
