using RabbitMQ.Client;

namespace Notification.Api.Services;

public class RabbitMqEventSubscriber(IConnection connection, IServiceScopeFactory scopeFactory) : IEventSubscriber
{
    public void Subscribe()
    {
    }
}
