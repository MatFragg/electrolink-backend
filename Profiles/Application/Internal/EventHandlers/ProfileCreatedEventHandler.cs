using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class ProfileCreatedEventHandler (ILogger<ProfileCreatedEventHandler> logger,IIntegrationEventPublisher integrationEventPublisher) : IEventHandler<ProfileCreatedEvent>
{
    public async Task Handle(ProfileCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: ProfileCreatedEvent received for ProfileId: {notification.ProfileId}, Role: {notification.Role}.");

        var integrationEvent = new ProfileCreatedIntegrationEvent(
            notification.ProfileId,
            notification.EmailAddress,
            notification.FullName,
            notification.Role.ToString(), 
            notification.OccurredOn
        );
        await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        logger.LogInformation($"[Profiles BC] Published Integration Event: ProfileCreatedIntegrationEvent for ProfileId: {notification.ProfileId}.");
    }
}