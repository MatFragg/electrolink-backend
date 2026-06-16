using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceAssignedToPropertyEventHandler : INotificationHandler<DeviceAssignedToPropertyEvent>
{
    private readonly ILogger<DeviceAssignedToPropertyEventHandler> _logger;

    public DeviceAssignedToPropertyEventHandler(ILogger<DeviceAssignedToPropertyEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceAssignedToPropertyEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Device {DeviceId} assigned to property {PropertyId} for installation request {InstallationRequestId}.",
            notification.DeviceId, notification.PropertyId, notification.InstallationRequestId);
        return Task.CompletedTask;
    }
}
