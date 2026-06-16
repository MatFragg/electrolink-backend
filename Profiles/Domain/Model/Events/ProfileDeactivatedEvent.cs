using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfileDeactivatedEvent : IEvent
{
    public Guid EventId { get; }

    public ProfileId ProfileId { get; }
    public UserId UserId { get; }
    public EBusinessRole BusinessRole { get; }
    public DateTime OccurredOn { get; }

    public ProfileDeactivatedEvent(ProfileId profileId, UserId userId, EBusinessRole businessRole)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        UserId = userId;
        BusinessRole = businessRole;
        OccurredOn = DateTime.UtcNow;
    }
}