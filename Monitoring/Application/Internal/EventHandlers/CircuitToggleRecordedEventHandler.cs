using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class CircuitToggleRecordedEventHandler(
    ILogger<CircuitToggleRecordedEventHandler> logger)
    : INotificationHandler<CircuitToggleRecordedEvent>
{
    public async Task Handle(CircuitToggleRecordedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] Circuit toggle recorded for device {DeviceId} (service {ExecutionId}), status: {ActionStatus}.",
            notification.DeviceId.Value, notification.ExecutionId.Value, notification.ActionStatus);
        await Task.CompletedTask;
    }
}
