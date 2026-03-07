using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class CompleteProfileAsHomeownerCommandFromResourceAssembler
{
    public static CompleteProfileAsHomeownerCommand ToCommandFromResource(
        CompleteProfileAsHomeownerResource resource,
        string userId)
    {
        var communicationPreferences = CommunicationPreferences.Create(
            resource.SmsNotifications,
            resource.EmailNotifications,
            resource.PushNotifications,
            resource.PreferredContactTime);

        EmergencyContact? emergencyContact = resource.EmergencyContact is null
            ? null
            : EmergencyContact.Create(
                resource.EmergencyContact.Name,
                resource.EmergencyContact.Relationship,
                resource.EmergencyContact.PhoneNumber);

        return new CompleteProfileAsHomeownerCommand(
            UserId:                   userId,
            FirstName:                resource.FirstName,
            LastName:                 resource.LastName,
            Email:                    resource.Email,
            PhoneNumber:              resource.PhoneNumber,
            Dni:                      resource.Dni,
            DateOfBirth:              resource.DateOfBirth,
            Street:                   resource.Street,
            District:                 resource.District,
            City:                     resource.City,
            Country:                  resource.Country,
            PostalCode:               resource.PostalCode,
            PreferredContactTime:     resource.PreferredContactTime,
            CommunicationPreferences: communicationPreferences,
            EmergencyContact:         emergencyContact);
    }
}

