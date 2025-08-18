using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class TechnicianSpecialtyUpdatedEventHandler( ILogger<TechnicianSpecialtyUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher) : IEventHandler<TechnicianSpecialtyUpdatedEvent>
{

    public async Task Handle(TechnicianSpecialtyUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: TechnicianSpecialtyUpdatedEvent received for Technician: {notification.TechnicianId}.");

        // Publicar evento de integración para Service Design and Planning
        var integrationEvent = new TechnicianSpecialtyUpdatedIntegrationEvent(
            notification.TechnicianId,
            notification.NewSpecialties,
            notification.OccurredOn
        );
        await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        logger.LogInformation($"[Profiles BC] Published Integration Event: TechnicianSpecialtyUpdatedIntegrationEvent for Technician: {notification.TechnicianId}.");

    }
}
