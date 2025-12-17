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
/// Handler que escucha PaymentProcessedEvent (Domain Event)
/// y publica PaymentFailedIntegrationEvent si el pago falló.
/// </summary>
public class PaymentProcessedIntegrationEventPublisher(ISubscriptionRepository subscriptionRepository, AppDbContext dbContext,ILogger<PaymentProcessedIntegrationEventPublisher> logger) 
    : INotificationHandler<PaymentProcessedEvent>
{
    
    public async Task Handle(PaymentProcessedEvent domainEvent, CancellationToken ct)
    {
        // Solo publicamos Integration Event si el pago FALLÓ
        if (domainEvent.Status != EPaymentStatus.Failed)
            return;

        logger.LogInformation(
            "Procesando PaymentProcessedEvent (FAILED): Subscription {SubscriptionId}",
            domainEvent.SubscriptionId);

        var subscription = await subscriptionRepository
            .FindBySubscriptionIdAsync(new SubscriptionId(domainEvent.SubscriptionId));

        if (subscription == null)
        {
            logger.LogWarning("Subscription {SubscriptionId} not found", domainEvent.SubscriptionId);
            return;
        }

        // Calcular próximo reintento (ejemplo: en 3 días)
        var nextRetryDate = DateTime.UtcNow.AddDays(3);

        var integrationEvent = new PaymentFailedIntegrationEvent(
            subscription.Id.Value,
            subscription.UserId.Value,
            domainEvent.Amount,
            "USD", // TODO: Obtener de la transacción
            domainEvent.GatewayTransactionId,
            nextRetryDate,
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
            "PaymentFailedIntegrationEvent guardado en Outbox para Subscription {SubscriptionId}",
            subscription.Id);
    }
}