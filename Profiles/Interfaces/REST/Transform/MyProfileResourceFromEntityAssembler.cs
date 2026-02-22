using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public class MyProfileResourceFromEntityAssembler
{
    public static MyProfileResource ToResourceFromEntity(Profile entity) => 
        new(
            ProfileId:    entity.ProfileId.Value,
            UserId:       entity.UserId.Value,
            Status:       entity.Status.ToString(),
            BusinessRole: entity.BusinessRole?.ToString(),
            PersonalData: entity.PersonalData is null ? null : MapPersonalData(entity.PersonalData),
            Technician:   entity.Technician   is null ? null : MapTechnician(entity.Technician),
            Homeowner:    entity.Homeowner    is null ? null : MapHomeowner(entity.Homeowner));

    private static PersonalDataResource MapPersonalData(PersonalData pd) =>
        new(pd.FirstName,
            pd.LastName,
            pd.PhoneNumber.Value,
            pd.Dni.Value,
            pd.DateOfBirth.Value.ToString("yyyy-MM-dd"),
            pd.Address.Street,
            pd.Address.District,
            pd.Address.City,
            pd.Address.Country,
            pd.Address.PostalCode
            );

    private static TechnicianProfileResource MapTechnician(Technician t) =>
        new(t.TechnicianId.Value,
            t.Specialties.Select(s => s.ToString()).ToList(),
            t.ExperienceYears,
            t.AboutMe);

    private static HomeownerProfileResource MapHomeowner(HomeOwner h) =>
        new(h.HomeownerId.Value,
            h.PreferredContactTime.ToString(),
            h.CommunicationPreferences.SmsNotifications,
            h.CommunicationPreferences.EmailNotifications,
            h.CommunicationPreferences.PushNotifications,
            h.EmergencyContact?.Name,
            h.EmergencyContact?.Relationship,
            h.EmergencyContact?.PhoneNumber
            );
}