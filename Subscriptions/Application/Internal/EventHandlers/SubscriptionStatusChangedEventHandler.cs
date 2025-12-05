using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionStatusChangedEvent"/> domain event.
/// </summary>
public class SubscriptionStatusChangedEventHandler : IEventHandler<SubscriptionStatusChangedEvent>
{
    private readonly ILogger<SubscriptionStatusChangedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ISubscriptionQueryService _subscriptionQueryService; // To get subscription for current plan
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionStatusChangedEventHandler(
        ILogger<SubscriptionStatusChangedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        ISubscriptionQueryService subscriptionQueryService,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _subscriptionQueryService = subscriptionQueryService;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionStatusChangedEventHandler CTOR] Instanciando SubscriptionStatusChangedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionStatusChangedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}, Nuevo Estado: {notification.NewStatus}.");

        // Fetch the subscription and plan to determine detailed status for integration event
        var subscription = await _subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(notification.SubscriptionId));
        if (subscription == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Subscription with ID {notification.SubscriptionId} not found for status changed event, cannot publish detailed integration event.");
            return;
        }

        var plan = await _planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));
        if (plan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Plan with ID {subscription.PlanId.Value} not found for Subscription {notification.SubscriptionId}, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on the actual plan and subscription state
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            notification.NewStatus.ToString(),
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}
