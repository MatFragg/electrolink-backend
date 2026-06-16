using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class CircuitToggleRequestedEventHandler(
    ILogger<CircuitToggleRequestedEventHandler> logger)
    : INotificationHandler<CircuitToggleRequestedEvent>
{
    public async Task Handle(CircuitToggleRequestedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] Circuit toggle requested for device {DeviceId} (service {ExecutionId}), target state: {TargetState}.",
            notification.DeviceId.Value, notification.ExecutionId.Value, notification.TargetState);
        await Task.CompletedTask;
    }
}
