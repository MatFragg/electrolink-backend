using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class ProfilePersonalDataUpdatedEvent : IEvent
{
    public Guid EventId { get; }

    public ProfileId ProfileId { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? PhoneNumber { get; }

    public DateTime OccurredOn { get; }

    public ProfilePersonalDataUpdatedEvent(
        ProfileId profileId,
        string? firstName,
        string? lastName,
        string? phoneNumber)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        OccurredOn = DateTime.UtcNow;
    }
}