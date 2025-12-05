using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

/// <summary>
/// Represents the event when a plan's details are updated (e.g., name, description, price, default status).
/// </summary>
public record PlanDetailsUpdatedEvent(
    Guid PlanId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    EMonetizationType MonetizationType,
    bool IsDefault,
    EUserRole TargetRole,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}