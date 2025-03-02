using MRSTW.Application.Common.Models;

namespace MRSTW.Application.Services;

public class MessageBusClient : IMessageBusClient
{
    public Task PublishNewEmailNotificationAsync(SendEmailRequest request)
    {
        // TODO Implement sending email logic here
        
        return Task.CompletedTask;
    }
}
