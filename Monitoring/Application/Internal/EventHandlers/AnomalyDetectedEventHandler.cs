using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class AnomalyDetectedEventHandler(ILogger<AnomalyDetectedEventHandler> logger)
    : INotificationHandler<ActiveServiceAnomalyAlertedEvent>
{
    public async Task Handle(ActiveServiceAnomalyAlertedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "[Monitoring BC] Anomaly detected on device {DeviceId} (service {ExecutionId}): {AnomalyType} - {Description}.",
            notification.DeviceId.Value, notification.ExecutionId.Value,
            notification.AnomalyType, notification.Description);
        await Task.CompletedTask;
    }
}
