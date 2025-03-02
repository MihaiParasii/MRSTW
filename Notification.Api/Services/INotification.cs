using Notification.Api.Models;

namespace Notification.Api.Services;

public interface INotification
{
    Task NotifyAsync(SendNotificationRequest request);
}
