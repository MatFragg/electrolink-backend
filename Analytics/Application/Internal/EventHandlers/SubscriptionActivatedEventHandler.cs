using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class SubscriptionActivatedEventHandler(
    ILogger<SubscriptionActivatedEventHandler> logger)
    : IEventHandler<SubscriptionActivatedEvent>
{
    public Task Handle(SubscriptionActivatedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Analytics BC] SubscriptionActivated: user={UserId}, plan={PlanType}",
            @event.UserId, @event.PlanType);

        return Task.CompletedTask;
    }
}
