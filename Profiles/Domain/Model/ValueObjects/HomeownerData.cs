using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public sealed record HomeownerData
{
    public EContactTime PreferredContactTime { get; init; }
    public CommunicationPreferences CommunicationPreferences { get; init; }
    public EmergencyContact? EmergencyContact { get; init; }

    private HomeownerData() { }

    public static HomeownerData Create(EContactTime preferredContactTime, CommunicationPreferences communicationPreferences, EmergencyContact? emergencyContact = null)
    {
        if (!communicationPreferences.HasAtLeastOneChannelActive())
            throw new AtLeastOneNotificationChannelRequiredException();

        return new HomeownerData
        {
            PreferredContactTime = preferredContactTime,
            CommunicationPreferences = communicationPreferences,
            EmergencyContact = emergencyContact
        };
    }
}