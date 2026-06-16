using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceDecommissionedEventHandler : INotificationHandler<DeviceDecommissionedEvent>
{
    private readonly ILogger<DeviceDecommissionedEventHandler> _logger;

    public DeviceDecommissionedEventHandler(ILogger<DeviceDecommissionedEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceDecommissionedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Device DECOMMISSIONED: {DeviceId} (SerialNumber: {SerialNumber}) | PreviousStatus: {PreviousStatus} | Reason: {Reason}. " +
            "Subscriptions BC should decrement activeDeviceCount. " +
            "IoT Monitoring BC should stop processing readings from this device.",
            notification.DeviceId, notification.SerialNumber,
            notification.PreviousStatus, notification.Reason);
        return Task.CompletedTask;
    }
}
