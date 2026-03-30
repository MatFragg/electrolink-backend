using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events;

public record ServiceRequestLimitReachedEvent(
    HomeownerId HomeownerId, 
    string PlanType,
    int MonthlyLimit,
    int CurrentUsage,
    DateTime OccurredOn
    ) : IEvent { 
    public Guid EventId { get; init; } = Guid.NewGuid(); 
}
