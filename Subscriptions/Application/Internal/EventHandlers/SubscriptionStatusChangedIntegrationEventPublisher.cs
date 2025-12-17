using System.Text.Json;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handler que escucha SubscriptionStatusChangedEvent (Domain Event)
/// y publica SubscriptionActivatedIntegrationEvent o SubscriptionCancelledIntegrationEvent
/// (Integration Events) hacia otros contextos vía Outbox Pattern.
/// </summary>
public class SubscriptionStatusChangedIntegrationEventPublisher(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository, AppDbContext dbContext, ILogger<SubscriptionStatusChangedIntegrationEventPublisher> logger) 
    : INotificationHandler<SubscriptionStatusChangedEvent>
{
    public async Task Handle(SubscriptionStatusChangedEvent domainEvent, CancellationToken ct)
    {
        logger.LogInformation(
            "Procesando SubscriptionStatusChangedEvent: Subscription {SubscriptionId} - {OldStatus} → {NewStatus}",
            domainEvent.SubscriptionId,
            domainEvent.OldStatus,
            domainEvent.NewStatus);

        // 1. Obtener información completa de la suscripción y plan
        var subscription = await subscriptionRepository
            .FindBySubscriptionIdAsync(new SubscriptionId(domainEvent.SubscriptionId));

        if (subscription == null)
        {
            logger.LogWarning("Subscription {SubscriptionId} not found", domainEvent.SubscriptionId);
            return;
        }

        var plan = await planRepository.FindByIdAsync(subscription.PlanId);
        if (plan == null)
        {
            logger.LogWarning("Plan {PlanId} not found", subscription.PlanId);
            return;
        }

        // 2. Determinar qué Integration Event publicar según el cambio de estado
        if (domainEvent.NewStatus == ESubscriptionStatus.Active && 
            domainEvent.OldStatus != ESubscriptionStatus.Active)
        {
            // Suscripción fue activada
            await PublishSubscriptionActivatedAsync(subscription, plan, ct);
        }
        else if (domainEvent.NewStatus == ESubscriptionStatus.Cancelled &&
                 domainEvent.OldStatus != ESubscriptionStatus.Cancelled)
        {
            // Suscripción fue cancelada
            await PublishSubscriptionCancelledAsync(subscription, ct);
        }
        else if (domainEvent.NewStatus == ESubscriptionStatus.Expired)
        {
            // Suscripción expiró
            await PublishSubscriptionCancelledAsync(subscription, ct, "Subscription expired");
        }
    }

    private async Task PublishSubscriptionActivatedAsync(
        Domain.Model.Aggregates.Subscription subscription,
        Domain.Model.Aggregates.Plan plan,
        CancellationToken ct)
    {
        var integrationEvent = new SubscriptionActivatedIntegrationEvent(
            subscription.Id.Value,
            subscription.UserId.Value,
            subscription.PlanId.Value,
            plan.Name,
            subscription.StartDate,
            subscription.EndDate,
            subscription.Status == ESubscriptionStatus.Trial,
            plan.Benefits.Select(b => b.Type).ToList(),
            DateTime.UtcNow
        );

        await SaveToOutboxAsync(integrationEvent, ct);

        logger.LogInformation(
            "SubscriptionActivatedIntegrationEvent guardado en Outbox para Subscription {SubscriptionId}",
            subscription.Id);
    }

    private async Task PublishSubscriptionCancelledAsync(
        Domain.Model.Aggregates.Subscription subscription,
        CancellationToken ct,
        string reason = "User initiated")
    {
        var integrationEvent = new SubscriptionCancelledIntegrationEvent(
            subscription.Id.Value,
            subscription.UserId.Value,
            subscription.CancellationEffectiveDate ?? subscription.EndDate,
            reason,
            DateTime.UtcNow
        );

        await SaveToOutboxAsync(integrationEvent, ct);

        logger.LogInformation(
            "SubscriptionCancelledIntegrationEvent guardado en Outbox para Subscription {SubscriptionId}",
            subscription.Id);
    }

    private async Task SaveToOutboxAsync(object integrationEvent, CancellationToken ct)
    {
        var eventType = integrationEvent.GetType();
        var typeName = $"{eventType.FullName}, {eventType.Assembly.GetName().Name}";

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeName,
            Content = JsonSerializer.Serialize(integrationEvent, eventType)
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, ct);
        // No llamamos a SaveChanges aquí - el UnitOfWork lo hará
    }
}