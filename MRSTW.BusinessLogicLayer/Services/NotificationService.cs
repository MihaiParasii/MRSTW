using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MRSTW.BusinessLogicLayer.Common.Models;
using RabbitMQ.Client;

namespace MRSTW.BusinessLogicLayer.Services;

public class NotificationService(IRabbitMqService rabbitMqService)
{
    public async Task NotifyAsync(SendEmailRequest request)
    {
        await rabbitMqService.SendMessageToNotificationServiceAsync(request);
    }
}

public interface IRabbitMqService
{
    Task SendMessageToNotificationServiceAsync(object obj);
    Task SendMessageToNotificationServiceAsync(string message);
}

public class RabbitMqService(IConfiguration configuration) : IRabbitMqService
{
    private readonly string _notificationQueue = configuration.GetConnectionString("RabbitMQMain-Notification")!;

    public async Task SendMessageToNotificationServiceAsync(object obj)
    {
        string message = JsonSerializer.Serialize(obj);
        await SendMessageToNotificationServiceAsync(message);
    }

    public async Task SendMessageToNotificationServiceAsync(string message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        await using var connection = await factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _notificationQueue ,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        byte[] body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "", routingKey: _notificationQueue, mandatory: true, body: body);
    }
}
