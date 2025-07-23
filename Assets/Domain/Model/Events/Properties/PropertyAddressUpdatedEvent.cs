using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;

public record PropertyAddressUpdatedEvent(
    PropertyId PropertyId,
    string NewAddress,
    decimal NewLatitude,
    decimal NewLongitude,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};