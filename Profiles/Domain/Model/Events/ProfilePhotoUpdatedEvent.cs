using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfilePhotoUpdatedEvent : IEvent
{
    public Guid EventId { get; }
    public ProfileId ProfileId { get; }
    public string PublicUrl { get; }
    public string ProviderId { get; }
    public DateTime OccurredOn { get; }

    public ProfilePhotoUpdatedEvent(ProfileId profileId, string publicUrl, string providerId)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        PublicUrl = publicUrl;
        ProviderId = providerId;
        OccurredOn = DateTime.UtcNow;
    }
}
