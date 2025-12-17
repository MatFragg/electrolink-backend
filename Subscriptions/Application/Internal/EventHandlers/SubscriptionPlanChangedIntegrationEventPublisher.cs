using System.Text.Json;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handler que escucha SubscriptionPlanChangedEvent (Domain Event)
/// y publica PlanChangedIntegrationEvent hacia otros contextos.
/// </summary>
public class SubscriptionPlanChangedIntegrationEventPublisher(IPlanRepository planRepository, AppDbContext dbContext, ILogger<SubscriptionPlanChangedIntegrationEventPublisher> logger) 
    : INotificationHandler<SubscriptionPlanChangedEvent>
{

    public async Task Handle(SubscriptionPlanChangedEvent domainEvent, CancellationToken ct)
    {
        logger.LogInformation(
            "Procesando SubscriptionPlanChangedEvent: Subscription {SubscriptionId} - {OldPlanId} → {NewPlanId}",
            domainEvent.SubscriptionId,
            domainEvent.OldPlanId,
            domainEvent.NewPlanId);

        var oldPlan = await planRepository.FindByIdAsync(domainEvent.OldPlanId);
        var newPlan = await planRepository.FindByIdAsync(domainEvent.NewPlanId);

        if (oldPlan == null || newPlan == null)
        {
            logger.LogWarning("Old or New Plan not found");
            return;
        }

        var integrationEvent = new PlanChangedIntegrationEvent(
            domainEvent.SubscriptionId,
            domainEvent.UserId.Value,
            domainEvent.OldPlanId.Value,
            oldPlan.Name,
            domainEvent.NewPlanId.Value,
            newPlan.Name,
            DateTime.UtcNow, // Effective date
            newPlan.Benefits.Select(b => b.Type).ToList(),
            DateTime.UtcNow
        );

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

        logger.LogInformation(
            "PlanChangedIntegrationEvent guardado en Outbox para Subscription {SubscriptionId}",
            domainEvent.SubscriptionId);
    }
}