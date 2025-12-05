using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionPlanChangedEvent"/> domain event.
/// </summary>
public class SubscriptionPlanChangedEventHandler : IEventHandler<SubscriptionPlanChangedEvent>
{
    private readonly ILogger<SubscriptionPlanChangedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ISubscriptionQueryService _subscriptionQueryService; // To get subscription for current plan
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionPlanChangedEventHandler(
        ILogger<SubscriptionPlanChangedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        ISubscriptionQueryService subscriptionQueryService,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _subscriptionQueryService = subscriptionQueryService;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionPlanChangedEventHandler CTOR] Instanciando SubscriptionPlanChangedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionPlanChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionPlanChangedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}, Plan Anterior: {notification.OldPlanId.Value}, Nuevo Plan: {notification.NewPlanId.Value}.");

        // Fetch the subscription and new plan to determine detailed status for integration event
        var subscription = await _subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(notification.SubscriptionId));
        if (subscription == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Subscription with ID {notification.SubscriptionId} not found for plan changed event, cannot publish detailed integration event.");
            return;
        }

        var newPlan = await _planQueryService.Handle(new GetPlanByIdQuery(notification.NewPlanId.Value));
        if (newPlan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] New Plan with ID {notification.NewPlanId.Value} not found for Subscription {notification.SubscriptionId}, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on the new plan and subscription state
        bool isPremium = newPlan.MonetizationType != EMonetizationType.Free;
        bool isCertified = newPlan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = newPlan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            subscription.Status.ToString(), // Use current subscription status
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}