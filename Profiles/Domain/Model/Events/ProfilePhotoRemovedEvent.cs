using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfilePhotoRemovedEvent : IEvent
{
    public Guid EventId { get; }
    public ProfileId ProfileId { get; }
    public string PreviousProviderId { get; }
    public DateTime OccurredOn { get; }

    public ProfilePhotoRemovedEvent(ProfileId profileId, string previousProviderId)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        PreviousProviderId = previousProviderId;
        OccurredOn = DateTime.UtcNow;
    }
}
