using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class PortfolioItemAddedEventHandler(ILogger<PortfolioItemAddedEventHandler> logger) : IEventHandler<PortfolioItemAddedEvent>
{
    public async Task Handle(PortfolioItemAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: PortfolioItemAddedEvent received for ProfileId: {notification.ProfileId}, WorkId: {notification.WorkId}, Title: '{notification.Title}'.");
        logger.LogInformation($"[Profiles BC] Updating materialized portfolio view for profile {notification.ProfileId}.");
    }
}