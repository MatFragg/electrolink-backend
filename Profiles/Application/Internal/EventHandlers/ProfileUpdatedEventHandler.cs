using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class ProfileUpdatedEventHandler(ILogger<ProfileUpdatedEventHandler> logger,
IIntegrationEventPublisher integrationEventPublisher) : IEventHandler<ProfileUpdatedEvent>
{
    public async Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: ProfileUpdatedEvent received for ProfileId: {notification.ProfileId}.");

        // Publicar evento de integración
        var integrationEvent = new ProfileUpdatedIntegrationEvent(
            notification.ProfileId, 
            notification.NewFullName, 
            notification.NewEmailAddress,
            notification.NewStreetAddress,
            notification.OccurredOn
        );
        await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        logger.LogInformation($"[Profiles BC] Published Integration Event: ProfileUpdatedIntegrationEvent for ProfileId: {notification.ProfileId}.");
    }
}