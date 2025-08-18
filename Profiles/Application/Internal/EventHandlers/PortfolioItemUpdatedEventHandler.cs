using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class PortfolioItemUpdatedEventHandler(ILogger<PortfolioItemUpdatedEventHandler> logger) : IEventHandler<PortfolioItemUpdatedEvent>
{
    public async Task Handle(PortfolioItemUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: PortfolioItemUpdatedEvent received for ProfileId: {notification.ProfileId}, WorkId: {notification.WorkId}, New Title: '{notification.NewTitle}'.");
        logger.LogInformation($"[Profiles BC] Re-indexing portfolio item {notification.WorkId} for profile {notification.ProfileId}.");
    }
}