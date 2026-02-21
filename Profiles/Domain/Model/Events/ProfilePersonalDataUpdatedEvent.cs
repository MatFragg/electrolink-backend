using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class ProfilePersonalDataUpdatedEvent
{
    public ProfileId ProfileId { get; }
    public PersonalData PersonalData { get; }

    // Parameterless ctor for serializers / frameworks
    public ProfilePersonalDataUpdatedEvent()
    {
        ProfileId = default!;
        PersonalData = default!;
    }

    public ProfilePersonalDataUpdatedEvent(ProfileId profileId, PersonalData personalData)
    {
        ProfileId = profileId ?? throw new ArgumentNullException(nameof(profileId));
        PersonalData = personalData ?? throw new ArgumentNullException(nameof(personalData));
    }
}