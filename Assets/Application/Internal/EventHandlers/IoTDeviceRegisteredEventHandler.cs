using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class IoTDeviceRegisteredEventHandler : INotificationHandler<IoTDeviceRegisteredEvent>
{
    private readonly ILogger<IoTDeviceRegisteredEventHandler> _logger;

    public IoTDeviceRegisteredEventHandler(ILogger<IoTDeviceRegisteredEventHandler> logger)
        => _logger = logger;

    public Task Handle(IoTDeviceRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "IoTDevice registered: {DeviceId} | SerialNumber: {SerialNumber} | Firmware: {FirmwareVersion}",
            notification.DeviceId, notification.SerialNumber, notification.FirmwareVersion);
        return Task.CompletedTask;
    }
}
