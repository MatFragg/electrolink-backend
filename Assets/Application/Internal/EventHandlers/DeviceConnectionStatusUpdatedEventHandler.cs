using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceConnectionStatusUpdatedEventHandler
    : INotificationHandler<DeviceConnectionStatusUpdatedEvent>
{
    private readonly ILogger<DeviceConnectionStatusUpdatedEventHandler> _logger;

    public DeviceConnectionStatusUpdatedEventHandler(
        ILogger<DeviceConnectionStatusUpdatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceConnectionStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Device {DeviceId} at property {PropertyId}: {Previous} -> {New}. LastReading: {LastReadingAt}. " +
            "Analytics BC should update the dashboard stream status.",
            notification.DeviceId, notification.PropertyId,
            notification.PreviousStatus, notification.NewStatus, notification.LastReadingAt);
        return Task.CompletedTask;
    }
}
