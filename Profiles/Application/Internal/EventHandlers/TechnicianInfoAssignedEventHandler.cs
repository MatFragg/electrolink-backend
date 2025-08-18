using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class TechnicianInfoAssignedEventHandler(ILogger<TechnicianInfoAssignedEventHandler> logger) : IEventHandler<TechnicianInfoAssignedEvent>
{
    public async Task Handle(TechnicianInfoAssignedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: TechnicianInfoAssignedEvent received for ProfileId: {notification.ProfileId}, License: {notification.LicenseNumber}.");
        logger.LogInformation($"[Profiles BC] Initiating license verification for Technician {notification.ProfileId}: {notification.LicenseNumber}.");
    }
}