using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfilePersonalDataUpdatedEvent : IEvent
{
    public Guid EventId { get; }

    public ProfileId ProfileId { get; }
    public PersonalData PersonalData { get; }

    public DateTime OccurredOn { get; }

    public ProfilePersonalDataUpdatedEvent(ProfileId profileId, PersonalData personalData)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        PersonalData = personalData;
        OccurredOn = DateTime.UtcNow;
    }
}