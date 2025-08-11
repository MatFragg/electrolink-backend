using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.IAM.Application.Internal.EventHandlers;

public class UsernameUpdatedEventHandler : IEventHandler<UsernameUpdatedEvent>
{
    private readonly ILogger<UsernameUpdatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public UsernameUpdatedEventHandler(ILogger<UsernameUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(UsernameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[IAM BC] Domain Event: UsernameUpdatedEvent recibido para usuario ID: {notification.UserId}. Username cambiado de '{notification.OldUsername}' a '{notification.NewUsername}'.");

        var integrationEvent = new UsernameUpdatedIntegrationEvent(
            notification.UserId,
            notification.NewUsername,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[IAM BC] Publicado Integration Event: UsernameUpdatedIntegrationEvent para usuario ID: {notification.UserId}.");
    }
}