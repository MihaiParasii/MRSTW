namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IRabbitMqService
{
    Task SendMessageToNotificationServiceAsync(object obj);
    Task SendMessageToNotificationServiceAsync(string message);
}
