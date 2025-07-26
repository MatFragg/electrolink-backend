namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
    string Payload { get; } // JSON serializado del evento
}