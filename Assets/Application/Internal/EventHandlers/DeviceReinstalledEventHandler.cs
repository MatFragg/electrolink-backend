using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceReinstalledEventHandler : INotificationHandler<DeviceReinstalledEvent>
{
    private readonly ILogger<DeviceReinstalledEventHandler> _logger;

    public DeviceReinstalledEventHandler(ILogger<DeviceReinstalledEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceReinstalledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Device {DeviceId} at property {PropertyId} REINSTALLED by technician {TechnicianId}. Firmware: {FirmwareVersion}.",
            notification.DeviceId, notification.PropertyId, notification.ReinstalledByTechnicianId, notification.FirmwareVersion);
        return Task.CompletedTask;
    }
}
