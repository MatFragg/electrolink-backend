using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;

public record PropertyDetailsUpdatedEvent(Guid Id, OwnerId OwnerId, Address Address, Region Region, District District, DateTime Updated): IEvent {
    public Guid EventId { get; init; } = Guid.NewGuid();
};