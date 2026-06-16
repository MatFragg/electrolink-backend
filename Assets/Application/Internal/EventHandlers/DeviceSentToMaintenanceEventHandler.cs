using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceSentToMaintenanceEventHandler : INotificationHandler<DeviceSentToMaintenanceEvent>
{
    private readonly ILogger<DeviceSentToMaintenanceEventHandler> _logger;

    public DeviceSentToMaintenanceEventHandler(ILogger<DeviceSentToMaintenanceEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceSentToMaintenanceEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Device {DeviceId} at property {PropertyId} sent to MAINTENANCE. Reason: {Reason}. Expected return: {ExpectedReturnDate}.",
            notification.DeviceId, notification.PropertyId, notification.Reason, notification.ExpectedReturnDate);
        return Task.CompletedTask;
    }
}
