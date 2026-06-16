using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceInstalledEventHandler : INotificationHandler<DeviceInstalledEvent>
{
    private readonly ILogger<DeviceInstalledEventHandler> _logger;

    public DeviceInstalledEventHandler(ILogger<DeviceInstalledEventHandler> logger)
        => _logger = logger;

    public Task Handle(DeviceInstalledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DeviceInstalled: {DeviceId} at property {PropertyId} by technician {TechnicianId}. " +
            "Subscriptions BC should activate EnterpriseSubscription for installationRequest {InstallationRequestId}. " +
            "IoT Monitoring BC should start awaiting readings from {DeviceId}.",
            notification.DeviceId, notification.PropertyId,
            notification.InstalledByTechnicianId, notification.InstallationRequestId,
            notification.DeviceId);
        return Task.CompletedTask;
    }
}
