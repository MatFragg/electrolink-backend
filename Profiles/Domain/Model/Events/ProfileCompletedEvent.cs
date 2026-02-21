using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfileCompletedEvent : IEvent
{
    public Guid EventId { get; }
    public ProfileId ProfileId { get; }
    public UserId UserId { get; }
    public EBusinessRole BusinessRole { get; }
    public object SubjectId { get; }
    public DateTime OccurredOn { get; }
    
    public ProfileCompletedEvent(ProfileId profileId, UserId userId, EBusinessRole businessRole, object subjectId)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        UserId = userId;
        BusinessRole = businessRole;
        SubjectId = subjectId;
        OccurredOn = DateTime.UtcNow;
    }
}