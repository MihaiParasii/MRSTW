using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notification.Api.Services;

public class RabbitMqListener : BackgroundService
{
    private readonly string _notificationQueue;

    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqListener(IConfiguration configuration)
    {
        _notificationQueue = configuration.GetConnectionString("RabbitMQMain-Notification")!;

        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.QueueDeclareAsync(queue: _notificationQueue, durable: false, exclusive: false, autoDelete: false,
            arguments: null).GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            string? content = Encoding.UTF8.GetString(ea.Body.ToArray());

            Debug.WriteLine($"Получено сообщение: {content}");

            await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(_notificationQueue, false, consumer, cancellationToken: stoppingToken);
    }

    public override void Dispose()
    {
        _channel.CloseAsync().GetAwaiter().GetResult();
        _connection.CloseAsync().GetAwaiter().GetResult();
        base.Dispose();
    }
}
