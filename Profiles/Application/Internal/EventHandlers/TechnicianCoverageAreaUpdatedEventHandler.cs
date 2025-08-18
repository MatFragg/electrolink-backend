using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class TechnicianCoverageAreaUpdatedEventHandler(ILogger<TechnicianCoverageAreaUpdatedEventHandler> logger,
IIntegrationEventPublisher integrationEventPublisher) : IEventHandler<TechnicianCoverageAreaUpdatedEvent>
{
    public async Task Handle(TechnicianCoverageAreaUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: TechnicianCoverageAreaUpdatedEvent received for Technician: {notification.TechnicianId}.");

        // Publicar evento de integración para Service Design and Planning
        var integrationEvent = new TechnicianCoverageAreaUpdatedIntegrationEvent(
            notification.TechnicianId,
            notification.NewCoverageAreaDetails,
            notification.OccurredOn
        );
        await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        logger.LogInformation($"[Profiles BC] Published Integration Event: TechnicianCoverageAreaUpdatedIntegrationEvent for Technician: {notification.TechnicianId}.");
    }
}