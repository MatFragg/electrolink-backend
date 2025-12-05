using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;


/// <summary>
/// Represents the event when a new plan is created.
/// </summary>
public record PlanCreatedEvent(
    Guid PlanId,
    string Name,
    decimal Price,
    EMonetizationType MonetizationType,
    EUserRole TargetRole,
    bool IsDefault,
    IEnumerable<Benefit> Benefits,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}