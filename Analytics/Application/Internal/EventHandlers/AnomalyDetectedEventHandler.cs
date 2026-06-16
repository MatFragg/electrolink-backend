using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class AnomalyDetectedEventHandler(
    IAlertLogCommandService alertLogCommandService,
    ILogger<AnomalyDetectedEventHandler> logger)
    : INotificationHandler<AnomalyDetectedIntegrationEvent>
{
    public async Task Handle(AnomalyDetectedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Analytics BC] AnomalyDetected: homeowner={HomeownerId}, type={Type}",
            notification.HomeownerId, notification.AnomalyType);

        await alertLogCommandService.RecordAnomalyAlertAsync(
            notification.HomeownerId,
            notification.AlertId,
            notification.Severity,
            notification.DeviceId);
    }
}
