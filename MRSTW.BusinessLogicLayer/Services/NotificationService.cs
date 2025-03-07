using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.BusinessLogicLayer.Services;

public class NotificationService(IBusinessUnitOfWork unitOfWork)
{
    public async Task NotifyAsync(SendEmailRequest request)
    {
        await unitOfWork.RabbitMqService.SendMessageToNotificationServiceAsync(request);
    }
}