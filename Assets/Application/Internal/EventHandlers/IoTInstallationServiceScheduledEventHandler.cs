using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class IoTInstallationServiceScheduledEventHandler
    : INotificationHandler<IoTInstallationServiceScheduledExternalEvent>
{
    private readonly IIoTDeviceCommandService _deviceCommandService;
    private readonly ILogger<IoTInstallationServiceScheduledEventHandler> _logger;

    public IoTInstallationServiceScheduledEventHandler(
        IIoTDeviceCommandService deviceCommandService,
        ILogger<IoTInstallationServiceScheduledEventHandler> logger)
    {
        _deviceCommandService = deviceCommandService;
        _logger = logger;
    }

    public async Task Handle(
        IoTInstallationServiceScheduledExternalEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "IoTInstallationServiceScheduled received for property {PropertyId} | InstallationRequest {RequestId}. " +
            "Assigning first available IN_STOCK device.",
            notification.PropertyId, notification.InstallationServiceRequestId);

        await _deviceCommandService.Handle(new AssignDeviceToPropertyCommand(
            DeviceId: notification.SelectedDeviceId,
            PropertyId: notification.PropertyId,
            InstallationRequestId: notification.InstallationServiceRequestId));
    }
}
