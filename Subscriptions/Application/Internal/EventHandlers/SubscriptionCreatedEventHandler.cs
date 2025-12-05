using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionCreatedEvent"/> domain event.
/// </summary>
public class SubscriptionCreatedEventHandler : IEventHandler<SubscriptionCreatedEvent>
{
    private readonly ILogger<SubscriptionCreatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionCreatedEventHandler(
        ILogger<SubscriptionCreatedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionCreatedEventHandler CTOR] Instanciando SubscriptionCreatedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionCreatedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}.");

        // Fetch the plan details to correctly populate the integration event
        var plan = await _planQueryService.Handle(new GetPlanByIdQuery(notification.PlanId.Value));
        if (plan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Plan with ID {notification.PlanId.Value} not found for SubscriptionCreatedEvent, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on plan benefits
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            notification.InitialStatus.ToString(),
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}