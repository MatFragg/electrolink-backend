using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.IAM.Application.Internal.EventHandlers;


public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[IAM BC] Domain Event: UserRegisteredEvent recibido para usuario ID: {notification.UserId}, Username: {notification.Username}.");

        var integrationEvent = new UserRegisteredIntegrationEvent(
            notification.UserId,
            notification.Username,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[IAM BC] Publicado Integration Event: UserRegisteredIntegrationEvent para usuario ID: {notification.UserId}.");
    }
}
