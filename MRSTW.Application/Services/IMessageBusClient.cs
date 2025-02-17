using MRSTW.Application.Common.Models;

namespace MRSTW.Application.Services;

public interface IMessageBusClient
{
    Task PublishNewEmailNotificationAsync(SendEmailRequest request);
}