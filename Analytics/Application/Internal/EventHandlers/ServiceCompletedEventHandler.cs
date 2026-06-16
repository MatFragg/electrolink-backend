using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class ServiceCompletedEventHandler(
    ITechnicianMetricsCommandService technicianMetricsCommandService,
    ILogger<ServiceCompletedEventHandler> logger)
    : INotificationHandler<ServiceCompletedIntegrationEvent>
{
    public async Task Handle(ServiceCompletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Analytics BC] ServiceCompleted: homeowner={HomeownerId}, service={ServiceId}, tech={TechnicianId}",
            notification.HomeownerId, notification.ServiceId, notification.TechnicianId);

        await technicianMetricsCommandService.UpdateTechnicianMetricsAsync(
            notification.TechnicianId,
            notification.HomeownerId,
            notification.ServiceRevenue,
            notification.Currency,
            notification.ResponseTime,
            notification.RequiresIoTCertification);
    }
}
