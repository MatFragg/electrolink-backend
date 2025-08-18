using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class HomeOwnerInfoAssignedEventHandler(ILogger<HomeOwnerInfoAssignedEventHandler> logger) : IEventHandler<HomeOwnerInfoAssignedEvent>
{ 

    public async Task Handle(HomeOwnerInfoAssignedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: HomeOwnerInfoAssignedEvent received for ProfileId: {notification.ProfileId}, DNI: {notification.Dni}.");
        logger.LogInformation($"[Profiles BC] Initiating verification process for DNI: {notification.Dni} of HomeOwner {notification.ProfileId}.");
    }
}