using System.Text.Json;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Hampcoders.Electrolink.API.Shared.Application.Internal.EventPublisher;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly AppDbContext _context;

    public IntegrationEventPublisher(AppDbContext context)
    {
        _context = context;
    }

    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        var outboxMessage = new OutboxMessage
        {
            Id = integrationEvent.EventId,
            OccurredOnUtc = integrationEvent.OccurredOn,
            Type = integrationEvent.GetType().Name, // O el nombre completo del tipo
            Content = JsonSerializer.Serialize((object)integrationEvent), // Serializar el evento a JSON
            ProcessedOnUtc = null,
            Error = null
        };

        _context.Set<OutboxMessage>().Add(outboxMessage);
        // NOTA: No llamamos a SaveChanges aquí. Esto lo hará la UnitOfWork del Command Service.
        // El OutboxMessage se guardará en la misma transacción que los cambios del Aggregate Root.
    }
}