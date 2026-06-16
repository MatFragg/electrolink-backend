using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record PropertyMainPhotoUpdatedEvent(
    PropertyId PropertyId,
    string ProviderId,
    string PublicUrl,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
