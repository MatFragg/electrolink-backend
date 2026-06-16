using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class RelayCommandExecutedEventHandler(
    IAlertLogCommandService alertLogCommandService,
    ILogger<RelayCommandExecutedEventHandler> logger)
    : INotificationHandler<RelayCommandExecutedIntegrationEvent>
{
    public async Task Handle(RelayCommandExecutedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Analytics BC] RelayCommandExecuted: homeowner={HomeownerId}, device={DeviceId}, circuit={CircuitId}",
            notification.HomeownerId, notification.DeviceId, notification.CircuitId);

        await alertLogCommandService.RecordCircuitToggleAlertAsync(
            notification.HomeownerId,
            notification.DeviceId,
            notification.CircuitId);
    }
}
