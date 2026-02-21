using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class HomeownerDataUpdatedEvent : IEvent
{
    public Guid EventId { get; }
    public ProfileId ProfileId { get; }
    public HomeownerId HomeownerId { get; }

    // Campos opcionales: null = no cambiado
    public EContactTime? PreferredContactTime { get; }
    public CommunicationPreferences? CommunicationPreferences { get; }
    public EmergencyContact? EmergencyContact { get; }

    public DateTime OccurredOn { get; }

    public HomeownerDataUpdatedEvent(
        ProfileId profileId,
        HomeownerId homeownerId,
        EContactTime? preferredContactTime,
        CommunicationPreferences? communicationPreferences,
        EmergencyContact? emergencyContact)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        HomeownerId = homeownerId;
        PreferredContactTime = preferredContactTime;
        CommunicationPreferences = communicationPreferences;
        EmergencyContact = emergencyContact;
        OccurredOn = DateTime.UtcNow;
    }
}