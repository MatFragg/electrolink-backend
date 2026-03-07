using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class UpdateHomeownerPreferencesCommandFromResourceAssembler
{
    public static UpdateCommunicationPreferencesCommand ToCommandFromResource(
        UpdateHomeownerPreferencesResource resource,
        string profileId,
        string userId)
    {
        EmergencyContact? emergencyContact = resource.EmergencyContact is null
            ? null
            : EmergencyContact.Create(
                resource.EmergencyContact.Name,
                resource.EmergencyContact.Relationship,
                resource.EmergencyContact.PhoneNumber);

        return new UpdateCommunicationPreferencesCommand(
            ProfileId:            profileId,
            UserId:               userId,
            SmsNotifications:     resource.SmsNotifications,
            EmailNotifications:   resource.EmailNotifications,
            PushNotifications:    resource.PushNotifications,
            PreferredContactTime: resource.PreferredContactTime,
            EmergencyContact:     emergencyContact);
    }
}

