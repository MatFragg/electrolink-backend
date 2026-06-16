using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ServiceCompletedIoTInstallationEventHandler
    : INotificationHandler<ServiceCompletedExternalEvent>
{
    private readonly IIoTDeviceCommandService _deviceCommandService;
    private readonly IPropertyCommandService _propertyCommandService;
    private readonly ILogger<ServiceCompletedIoTInstallationEventHandler> _logger;

    public ServiceCompletedIoTInstallationEventHandler(
        IIoTDeviceCommandService deviceCommandService,
        IPropertyCommandService propertyCommandService,
        ILogger<ServiceCompletedIoTInstallationEventHandler> logger)
    {
        _deviceCommandService = deviceCommandService;
        _propertyCommandService = propertyCommandService;
        _logger = logger;
    }

    public async Task Handle(
        ServiceCompletedExternalEvent notification,
        CancellationToken cancellationToken)
    {
        if (notification.ServiceType != "IOT_INSTALLATION") return;

        _logger.LogInformation(
            "ServiceCompleted (IOT_INSTALLATION) for device {DeviceId} at property {PropertyId}.",
            notification.IoTDeviceId, notification.PropertyId);

        await _deviceCommandService.Handle(new RecordDeviceInstallationCommand(
            DeviceId: notification.IoTDeviceId!,
            PropertyId: notification.PropertyId,
            TechnicianId: notification.TechnicianId,
            InstallationRequestId: notification.ServiceId,
            FirmwareVersion: notification.FirmwareVersion ?? "unknown",
            InstalledAt: notification.CompletedAt));

        await _propertyCommandService.Handle(new RecordMaintenanceForPropertyCommand(
            PropertyId: PropertyId.From(notification.PropertyId),
            AssignmentId: AssignmentId.From(notification.ServiceId),
            TechnicianId: TechnicianId.From(notification.TechnicianId),
            WorkSummary: $"IoT device {notification.IoTDeviceId} installed.",
            CompletedAt: notification.CompletedAt));
    }
}
